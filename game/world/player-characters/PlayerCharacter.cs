using Godot;

namespace Lastdew
{	
	public partial class PlayerCharacter : CharacterBody3D
	{
		[Export]
		public new string Name { get; set; }
		[Export]
		public Texture2D Icon { get; set; }
		public PcHealth Health { get; private set; }
		// TODO: How to integrate stats/equipment into combat/health?
		// Inventory is shared, but equipment and stats should be on the individual pc.
		public PcStatManager StatManager { get; set; }
		public PcEquipment Equipment { get; set; }
		private PcStateMachine StateMachine { get; set; }
		private InventoryManager Inventory { get; set; }
	
		public void Initialize(InventoryManager inventoryManager)
		{
			PcStateContext context = new(this, inventoryManager);
			StateMachine = new PcStateMachine(context);
			Health = new PcHealth();
			StatManager = new PcStatManager(new PcStatsData());
			Equipment = new PcEquipment();
			Inventory = inventoryManager;
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
		
		/// <returns>true if pc incapacitated</returns>
		public bool GetHit(Enemy attackingEnemy, int damage)
		{
			bool incapacitated = Health.TakeDamage(damage);
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
		
		public void ExitTree()
		{
			StateMachine.ExitTree();
		}
	}
}
