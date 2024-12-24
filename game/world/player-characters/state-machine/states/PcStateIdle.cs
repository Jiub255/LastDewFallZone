
namespace Lastdew
{	public class PcStateIdle : PcState
	{
		public PcStateIdle(PcStateContext context) : base(context) {}
	
		public override void EnterState(MovementTarget target) {}
		public override void ExitState() {}
		public override void PhysicsProcessSelected(float delta) {}
		public override void PhysicsProcessUnselected(float delta) {}
		public override void ProcessSelected(float delta) {}
		public override void ProcessUnselected(float delta) {}
	}
}
