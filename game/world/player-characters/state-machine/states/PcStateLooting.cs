using Godot;
using System;

public class PcStateLooting : PcState
{
	public PcStateLooting(PcStateContext context) : base(context) {}

	public override void EnterState()
	{
		Context.PcAnimationTree.Looting = true;
	}

	public override void ExitState()
	{
		Context.PcAnimationTree.Looting = false;
	}

	public override void PhysicsProcessUnselected(float delta)
	{
		
	}

	public override void ProcessUnselected(float delta)
	{
		
	}

	public override void ProcessSelected(float delta)
	{
		
	}

	public override void PhysicsProcessSelected(float delta)
	{
		
	}
}
