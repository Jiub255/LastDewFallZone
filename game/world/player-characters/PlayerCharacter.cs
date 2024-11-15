using Godot;

public partial class PlayerCharacter : CharacterBody3D
{
	private PcStateMachine StateMachine { get; set; }
	public PcHealth Health { get; private set; }

	public override void _Ready()
	{
		base._Ready();

	}
	
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
		StateMachine?.ProcessStateUnselected((float)delta);
		Health.ProcessRelief((float)delta);
	}

	public void PhysicsProcessSelected(double delta)
	{
		StateMachine?.PhysicsProcessStateUnselected((float)delta);
	}
	
	public void MoveTo(MovementTarget movementTarget)
	{
		StateMachine.ChangeState(PcStateNames.MOVEMENT, movementTarget);
	}
	
	public void ExitTree()
	{
		StateMachine.ExitTree();
	}
}
