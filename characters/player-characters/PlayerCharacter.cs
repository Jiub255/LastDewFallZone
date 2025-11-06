using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Lastdew
{
	public partial class PlayerCharacter : CharacterBody3D
	{
		public event Action OnEquipmentChanged;
		public event Action OnDeath;
		
		private const float SIGHT_DISTANCE = 20f;
		
		private MovementTarget _movementTarget;

		public MovementTarget MovementTarget
		{
			get => _movementTarget;
			set
			{
				if (_movementTarget.Target is Enemy oldEnemy)
				{
					oldEnemy.OnDeath -= OnTargetDeath;
				}
				_movementTarget = value;
				if (_movementTarget.Target is Enemy newEnemy)
				{
					//this.PrintDebug($"{Name}'s target set to {newEnemy.Name}");
					newEnemy.OnDeath += OnTargetDeath;
				}
			}
		}
		public bool Incapacitated { get; private set; }
		public new string Name { get; private set; }
		public Texture2D Icon { get; private set; }
		public PcHealth Health { get; private set; }
		public PcStatManager StatManager { get; private set; }
		public PcEquipment Equipment { get; private set; }
		public NavigationAgent3D NavigationAgent { get; private set; }
		public AnimationTree PcAnimationTree { get; private set; }
		public AnimationNodeStateMachinePlayback AnimStateMachine { get; private set; }
		
		private PcStateMachine StateMachine { get; set; }
		private InventoryManager Inventory { get; set; }
		
		public void Initialize(InventoryManager inventoryManager, PcSaveData saveData)
		{
			NavigationAgent = GetNode<NavigationAgent3D>("%NavigationAgent3D");
			PcAnimationTree = GetNode<AnimationTree>("%AnimationTree");
			AnimStateMachine = (AnimationNodeStateMachinePlayback)PcAnimationTree.Get("parameters/playback");
			
			StateMachine = new PcStateMachine(this);
			Health = new PcHealth(saveData);
			StatManager = new PcStatManager();
			Equipment = new PcEquipment(saveData);
			Inventory = inventoryManager;

			SetupPcData(saveData.Name);

			Health.OnHealthChanged += StatManager.SetPain;
		}

        public void ProcessUnselected(double delta)
		{
			StateMachine?.ProcessStateUnselected((float)delta);
			Health.ProcessRelief((float)delta);
		}
	
		public void PhysicsProcessUnselected(double delta)
		{
			StateMachine?.PhysicsProcessStateUnselected((float)delta);
		}
		
		public void ProcessSelected(double delta)
		{
			StateMachine?.ProcessStateSelected((float)delta);
			Health.ProcessRelief((float)delta);
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
			int actualDamage = Mathf.Max(0, damage - StatManager.Defense);
			bool incapacitated = Health.TakeDamage(actualDamage);
			//this.PrintDebug($"{GetRid()} took {damage} damage");
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
			StateMachine.ExitTree();
			
			Health.OnHealthChanged -= StatManager.SetPain;
		}
		
		public PcSaveData GetSaveData()
		{
			return new PcSaveData(
				Name,
				Equipment.Weapon.GetUid(),
				Equipment.Head.GetUid(),
				Equipment.Body.GetUid(),
				Equipment.Feet.GetUid(),
				Health.Injury);
		}
		
		// Called from animation method track
		private void HitEnemy()
		{
			StateMachine.HitEnemy();
		}

        private void SetupPcData(string name)
        {
			PcData data = Databases.PcDatas[name];
			
			Name = name;
			Icon = data.Icon;
			
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
			meshes[(int)data.HeadMesh * 4 - 1].Show();
			meshes[(int)data.BodyMesh * 4 - 2].Show();
			meshes[(int)data.LegsMesh * 4 - 3].Show();
			meshes[(int)data.FeetMesh * 4 - 4].Show();
			// TODO: Kinda hacky, could do better. Doesn't matter. Just doing this to free up some memory why not.
            foreach (Node node in meshParent.GetChildren())
            {
                if (node is MeshInstance3D { Visible: false } mesh)
                {
					mesh.QueueFree();
                }
            }
        }

		private void OnTargetDeath()
		{
			TryFindNearestEnemy();
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
		        if (collider is not Enemy enemy || enemy.Health <= 0)
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
	}
}
