using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace Lastdew
{
	public partial class PlayerCharacter : CharacterBody3D
	{
		public event Action OnEquipmentChanged;
		public event Action OnDeath;
		
		private const float INVULNERABILITY_DURATION = 1f;
		private const float SIGHT_DISTANCE = 20f;
		
		private MovementTarget _movementTarget;

		public MovementTarget MovementTarget
		{
			get => _movementTarget;
			set
			{
				if (_movementTarget.Target is Enemy oldEnemy)
				{
					oldEnemy.OnDeath -= TryFindNearestEnemy;
				}
				_movementTarget = value;
				if (_movementTarget.Target is Enemy newEnemy)
				{
					//this.PrintDebug($"{Name}'s target set to {newEnemy.Name}");
					newEnemy.OnDeath += TryFindNearestEnemy;
				}
			}
		}
		[Export]
		public PcData Data { get; private set; }
		public PcStatManager StatManager { get; private set; }
		public PcEquipment Equipment { get; private set; }
		public NavigationAgent3D NavigationAgent { get; private set; }
		public AnimationTree PcAnimationTree { get; private set; }
		public AnimationNodeStateMachinePlayback AnimStateMachine { get; private set; }
		public bool Incapacitated { get; private set; }
		public bool Invulnerable { get; set; }
		
		private PcStateMachine StateMachine { get; set; }
		private InventoryManager Inventory { get; set; }
		private MeshInstance3D SelectedIndicator { get; set; }
		private float InvulnerabilityTimer { get; set; }
		private PackedScene NumberPopupScene { get; } = GD.Load<PackedScene>(Uids.NUMBER_POPUP);
		
		// TODO: Initialize a new PC with just PcData, not PcSaveData.
		// Maybe just immediately make a new PcSaveData when you get a new PC, so Initialize can still be used.
		public void SetupNewPc(InventoryManager inventoryManager, PcData pcData)
		{
			
		}
		
		public /*async Task*/ void Initialize(InventoryManager inventoryManager, PcSaveData saveData)
		{
			NavigationAgent = GetNode<NavigationAgent3D>("%NavigationAgent3D");
			PcAnimationTree = GetNode<AnimationTree>("%AnimationTree");
			AnimStateMachine = (AnimationNodeStateMachinePlayback)PcAnimationTree.Get("parameters/playback");
			SelectedIndicator = GetNode<MeshInstance3D>("%SelectedIndicator");
			
			StateMachine = new PcStateMachine(this);
			StatManager = new PcStatManager(saveData);
			Equipment = new PcEquipment(saveData);
			Inventory = inventoryManager;

			StatManager.Experience.OnExperienceGained += ShowPopup;

			/*await */SetupPcData(saveData.PcData);
		}

        public void ProcessUnselected(double delta)
        {
	        StateMachine?.ProcessStateUnselected((float)delta);
	        SharedProcess(delta);
        }

		public void PhysicsProcessUnselected(double delta)
		{
			StateMachine?.PhysicsProcessStateUnselected((float)delta);
		}
		
		public void ProcessSelected(double delta)
		{
			StateMachine?.ProcessStateSelected((float)delta);
			SharedProcess(delta);
		}
	
		public void PhysicsProcessSelected(double delta)
		{
			StateMachine?.PhysicsProcessStateSelected((float)delta);
		}
		
		public void MoveTo(MovementTarget movementTarget)
		{
			MovementTarget = movementTarget;
			StateMachine.ChangeState(PcStateNames.MOVEMENT);
		}

		/// TODO: Make actual defense formula.
		public void GetHit(Enemy attackingEnemy, int damage)
		{
			if (MovementTarget.Target is not Enemy)
			{
				MovementTarget = new MovementTarget(attackingEnemy.Position, attackingEnemy);
			}
			
			if (Invulnerable)
			{
				return;
			}
			
			int actualDamage = Mathf.Max(0, damage - StatManager.Defense);
			this.PrintDebug($"{Data.Name} hit by {attackingEnemy.Data.EnemyType} " +
			                $"{attackingEnemy.Name} for {actualDamage} damage.\n" +
			                $"Injury: {StatManager.Health.Injury}");
			bool incapacitated = StatManager.Health.TakeDamage(actualDamage);
			StateMachine.GetHit(incapacitated);
			if (incapacitated)
			{
				OnDeath?.Invoke();
			}
		}
		
		public void Equip(Equipment equipment)
		{
			if (!StatManager.MeetsRequirements(equipment))
			{
				return;
			}
			if (Inventory.HasItem(equipment))
			{
				Inventory.RemoveItem(equipment);
			}
			else
			{
				GD.PushError($"{equipment.Name} not in inventory");
				return;
			}
			Equipment oldEquipment = Equipment.Equip(equipment);
			StatManager.CalculateStatModifiers(Equipment.Bonuses);
			if (oldEquipment != null)
			{
				Inventory.AddItem(oldEquipment);
			}
			// TODO: Send equipment changed signal here for UI to catch?
			// OR, just structure things differently? Like have a database with id and
			// equipment columns and just have the ui hold on to that data?
			OnEquipmentChanged?.Invoke();
		}
		
		public void Unequip(EquipmentType equipmentType)
		{
			Equipment oldEquipment = Equipment.Unequip(equipmentType);
			StatManager.CalculateStatModifiers(Equipment.Bonuses);
			if (oldEquipment != null)
			{
				Inventory.AddItem(oldEquipment);
			}
		}
		
		public void UseItem(UsableItem item)
		{
			this.PrintDebug($"Using {item.Name}, effects: {item.Effects.Count}");
			foreach (Effect effect in item.Effects)
			{
				this.PrintDebug($"Effect: {effect}");
				effect.ApplyEffect(this);
			}
			if (!item.Reusable)
			{
				Inventory.RemoveItem(item);
			}
		}

		public void CollectLoot(Item item, int amount)
		{
			Inventory.AddItems(item, amount);
		}

		public void DisablePc()
		{
			Incapacitated = true;
			MovementTarget = new MovementTarget();
			CollisionLayer = 0;
			//NavigationAgent.AvoidanceEnabled = false;
			// TODO: Let PcManager (or TeamData) know Pc is dead so it can deselect it if it's selected.
		}
		
		public void ExitTree()
		{
			StatManager.Experience.OnExperienceGained -= ShowPopup;
			
			StateMachine.ExitTree();
			StatManager.ExitTree();
		}
		
        public void SetSelectedIndicator(bool on)
        {
	        SelectedIndicator.Visible = on;
        }
		
		// Called from animation method track
		private void HitEnemy()
		{
			StateMachine.HitEnemy();
		}

		private void SharedProcess(double delta)
		{
			StatManager.Health.ProcessRelief((float)delta);
			TickInvulnerability((float)delta);
		}

		private void TickInvulnerability(float delta)
		{
			if (!Invulnerable)
			{
				return;
			}
			InvulnerabilityTimer += delta;
			if (InvulnerabilityTimer >= INVULNERABILITY_DURATION)
			{
				InvulnerabilityTimer = 0;
				Invulnerable = false;
			}
		}

        private /*async Task*/ void SetupPcData(PcData data)
        {
			Data = data;
			
			Skeleton3D meshParent = GetNode<Skeleton3D>("%Skeleton3D");
			List<MeshInstance3D> meshes = [];
            foreach (Node node in meshParent.GetChildren())
            {
                if (node is MeshInstance3D mesh)
                {
                    meshes.Add(mesh);
					mesh.Hide();
                }
            }
			meshes[(int)Data.HeadMesh * 4 - 1].Show();
			meshes[(int)Data.BodyMesh * 4 - 2].Show();
			meshes[(int)Data.LegsMesh * 4 - 3].Show();
			meshes[(int)Data.FeetMesh * 4 - 4].Show();
			
            foreach (Node node in meshParent.GetChildren())
            {
                if (node is MeshInstance3D { Visible: false } mesh)
                {
					mesh.QueueFree();
                }
            }
            
			//await GetMugshotIcon();
        }

		// TODO: Data.Icon never gets set.
        private async Task GetMugshotIcon()
        {
	        Camera3D camera = GetNode<Camera3D>("%MugshotCamera");
	        await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
	        await GetTree().ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	        camera.Current = true;
	        await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
	        await GetTree().ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	        
	        Image screenshot = GetViewport().GetTexture().GetImage();
	        //camera.Current = false;
	        Image image = Image.CreateEmpty(648, 648, false, screenshot.GetFormat());
	        image.BlitRect(
		        screenshot,
		        new Rect2I(252, 0, 648, 648),
		        Vector2I.Zero);
	        image.Resize(128, 128, Image.Interpolation.Trilinear);
	        string path = $"res://characters/player-characters/management/pc_data/{Data.Name.ToSnakeCase()}.png";
	        Error error = image.SavePng(path);
	        this.PrintDebug($"image {image} saved to {path}");
	        if (error != Error.Ok)
	        {
		        GD.PushError($"Error saving png: {error}");
	        }

	        System.Threading.Thread.Sleep(1000);
	        
	        Texture2D texture = GD.Load<Texture2D>(path);
	        if (texture != null)
	        {
		        this.PrintDebug($"texture: {texture}");
				Data.Icon = texture;
	        }
	        
	        camera.QueueFree();
        }

        private void TryFindNearestEnemy()
        {
	        Enemy nearest = FindNearestEnemy();
	        
	        if (nearest != null)
	        {
		        MovementTarget = new MovementTarget(nearest.GlobalPosition, nearest);
		        StateMachine.ChangeState(PcStateNames.MOVEMENT);
	        }
	        else
	        {
		        MovementTarget = new MovementTarget();
		        StateMachine.ChangeState(PcStateNames.IDLE);
	        }
        }
        
        private Enemy FindNearestEnemy()
        {
	        Array<Dictionary> sphereCastResults = SphereCast();

	        Enemy closest = null;
	        foreach (Dictionary dict in sphereCastResults)
	        {
		        CollisionObject3D collider = (CollisionObject3D)dict["collider"];
		        if (collider is not Enemy { Health: > 0 } enemy)
		        {
			        continue;
		        }
		        if (closest == null || GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition) <
		            GlobalPosition.DistanceSquaredTo(closest.GlobalPosition))
		        {
			        closest = enemy;
		        }
	        }
	        return closest;
        }

        private Array<Dictionary> SphereCast()
        {
	        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
	        SphereShape3D sphereShape = new() { Radius = SIGHT_DISTANCE };
	        PhysicsShapeQueryParameters3D query = new()
	        {
		        ShapeRid = sphereShape.GetRid(),
		        CollideWithBodies = true,
		        Transform = new Transform3D(Basis.Identity, GlobalPosition),
		        Exclude = new Array<Rid>(new Rid[1] { GetRid() }),
		        CollisionMask = 0b100
	        };

	        Array<Dictionary> results = spaceState.IntersectShape(query);
	        return results;
        }

        private void ShowPopup(string text)
        {
	        NumberPopup3d popup = (NumberPopup3d)NumberPopupScene.Instantiate();
	        CallDeferred(Node.MethodName.AddChild, popup);
	        popup.CallDeferred(NumberPopup3d.MethodName.Show, text);
        }
	}
}
