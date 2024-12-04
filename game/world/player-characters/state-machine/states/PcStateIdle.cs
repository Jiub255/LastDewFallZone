public class PcStateIdle : PcState
{
	public PcStateIdle(PcStateContext context) : base(context) {}

	public override void EnterState(MovementTarget target)
	{
		Context.PcAnimationTree.Set(BlendAmountPath, 0);
	}
	public override void ExitState() {}
	public override void PhysicsProcessSelected(float delta) {}
	public override void PhysicsProcessUnselected(float delta) {}
	public override void ProcessSelected(float delta) {}
	public override void ProcessUnselected(float delta) {}
}
