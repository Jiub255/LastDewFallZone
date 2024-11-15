using System;

public abstract class PcState
{
	public event Action<PcStateNames, object> OnChangeState;
	
	protected PcStateContext Context { get; private set; }
	protected string BlendAmountPath { get; } = "parameters/movement_blend_tree/idle_move/blend_amount";
	
	public PcState(PcStateContext context)
	{
		Context = context;
	}

	public abstract void EnterState(object target = null);
	public abstract void ExitState();
	public abstract void ProcessUnselected(float delta);
	public abstract void PhysicsProcessUnselected(float delta);
	public abstract void ProcessSelected(float delta);
	public abstract void PhysicsProcessSelected(float delta);
	
	protected void ChangeState(PcStateNames stateName, object target = null)
	{
		OnChangeState?.Invoke(stateName, target);
	}
}
