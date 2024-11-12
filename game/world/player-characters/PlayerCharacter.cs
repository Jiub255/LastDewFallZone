using Godot;

public partial class PlayerCharacter : CharacterBody3D
{
	private PcStateMachine StateMachine { get; set; }

	public override void _Ready()
	{
		base._Ready();

		PcStateContext context = new(this);
		StateMachine = new PcStateMachine(context);
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		StateMachine.ExitTree();
	}

	// TODO: Run state stuff that should happen to non selected characters here, 
	// then run only selected char state stuff from PcManager?
	// Like have a SelectedProcess(double delta) and SelectedPhysicsProcess(double delta),
	// and put the selected logic in there. But how to avoid running the non-selected processes?
	public void ProcessUnselected(double delta)
	{
		StateMachine?.ProcessStateUnselected((float)delta);
	}

	public void PhysicsProcessUnselected(double delta)
	{
		StateMachine?.PhysicsProcessStateUnselected((float)delta);
	}
	
	public void ProcessSelected(double delta)
	{
		StateMachine?.ProcessStateUnselected((float)delta);
	}

	public void PhysicsProcessSelected(double delta)
	{
		StateMachine?.PhysicsProcessStateUnselected((float)delta);
	}
	
	public void MoveTo(Vector3 location)
	{
		StateMachine.MoveTo(null, location);
	}
	
	public void MoveTo(Node3D target)
	{
		StateMachine.MoveTo(target);
	}
}
