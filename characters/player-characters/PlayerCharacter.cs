using System;
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class PlayerCharacter : CharacterBody3D
	{
		public event Action OnEquipmentChanged;
		
		public bool Incapacitated { get; set; }
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
			StateMachine.ChangeState(PcStateNames.MOVEMENT, movementTarget);
		}

		/// <summary>
		/// TODO: Make actual defense formula.
		/// </summary>
		/// <returns>true if pc incapacitated</returns>
		public bool GetHit(Enemy attackingEnemy, int damage)
		{
			int actualDamage = Mathf.Max(0, damage - StatManager.Defense);
			bool incapacitated = Health.TakeDamage(actualDamage);
			this.PrintDebug($"{GetRid()} took {damage} damage");
			StateMachine.GetHit(attackingEnemy, incapacitated);
			return incapacitated;
		}
		
		// Called from animation method track
		public void HitEnemy()
		{
			StateMachine.HitEnemy(this);
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
			CollisionLayer = 0;
			NavigationAgent.AvoidanceEnabled = false;
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
	}
}
