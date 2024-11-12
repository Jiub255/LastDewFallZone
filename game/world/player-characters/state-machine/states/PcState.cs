using Godot;
using System;

public abstract class PcState
{
	public event Action<PcStateNames, Node3D> OnChangeState;

	public Node3D Target { get; set; }
	
	protected PcStateContext Context { get; private set; }
	
	public PcState(PcStateContext context)
	{
		Context = context;
	}

	public abstract void EnterState();
	public abstract void ExitState();
	public abstract void ProcessUnselected(float delta);
	public abstract void PhysicsProcessUnselected(float delta);
	public abstract void ProcessSelected(float delta);
	public abstract void PhysicsProcessSelected(float delta);
	
	protected virtual void ChangeState(PcStateNames state, Node3D target)
	{
		OnChangeState?.Invoke(state, target);
	}
}
