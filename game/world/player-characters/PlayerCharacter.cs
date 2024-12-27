using Godot;

namespace Lastdew
{	
	public partial class PlayerCharacter : CharacterBody3D
	{
		private PcStateMachine StateMachine { get; set; }
		public PcHealth Health { get; private set; }
	
		public void Initialize(InventoryManager inventoryManager)
		{
			PcStateContext context = new(this, inventoryManager);
			StateMachine = new PcStateMachine(context);
			Health = new PcHealth();
		}
	
		// TODO: Run state stuff that should happen to non selected characters here, 
		// then run only selected char state stuff from PcManager?
		// Like have a SelectedProcess(double delta) and SelectedPhysicsProcess(double delta),
		// and put the selected logic in there. But how to avoid running the non-selected processes?
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
		
		public void ExitTree()
		{
			StateMachine.ExitTree();
		}
	}
}
