using System;
using Godot;

namespace Lastdew
{	
	public partial class PlayerCharacter : CharacterBody3D
	{
		public event Action OnEquipmentChanged;
		
		[Export]
		public new string Name { get; set; }
		[Export]
		public Texture2D Icon { get; set; }
		public PcHealth Health { get; private set; }
		public PcStatManager StatManager { get; set; }
		public PcEquipment Equipment { get; set; }
		private PcStateMachine StateMachine { get; set; }
		private InventoryManager Inventory { get; set; }
	
		public void Initialize(InventoryManager inventoryManager, PcSaveData saveData)
		{
			PcStateContext context = new(this, inventoryManager);
			StateMachine = new PcStateMachine(context);
			Health = new PcHealth(saveData);
			StatManager = new PcStatManager();
			Equipment = new PcEquipment(saveData);
			Inventory = inventoryManager;

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
			int actualDamage = damage - StatManager.Defense;
			bool incapacitated = Health.TakeDamage(actualDamage);
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
			this.PrintDebug($"Using {item.Name}, effects: {item.Effects.Length}");
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
		
		public void ExitTree()
		{
			StateMachine.ExitTree();
			
			Health.OnHealthChanged -= StatManager.SetPain;
		}
		
		public PcSaveData GetSaveData()
		{
			return new PcSaveData(
				Name,
				Equipment.Head?.Name,
				Equipment.Weapon?.Name,
				Equipment.Body?.Name,
				Equipment.Feet?.Name,
				Health.Injury);
		}
	}
}
