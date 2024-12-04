using System;

public abstract class PcState
{
	public event Action<PcStateNames, MovementTarget> OnChangeState;
	
	protected PcStateContext Context { get; private set; }
	protected string BlendAmountPath { get; } = "parameters/movement_blend_tree/idle_move/blend_amount";
	
	public PcState(PcStateContext context)
	{
		Context = context;
	}

	public abstract void EnterState(MovementTarget target);
	public abstract void ExitState();
	public abstract void ProcessUnselected(float delta);
	public abstract void PhysicsProcessUnselected(float delta);
	public abstract void ProcessSelected(float delta);
	public abstract void PhysicsProcessSelected(float delta);
	
	protected void ChangeState(PcStateNames stateName, MovementTarget target)
	{
		OnChangeState?.Invoke(stateName, target);
	}
}
