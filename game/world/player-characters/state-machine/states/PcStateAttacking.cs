using System;

namespace Lastdew
{
	public partial class PcStateAttacking : PcState<PcCombatSubstateNames>
	{
		public PcStateAttacking(PcStateContext context) : base(context)
		{
		}

		public override void EnterState(MovementTarget target)
		{
			throw new NotImplementedException();
		}

		public override void ExitState()
		{
			throw new NotImplementedException();
		}

		public override void PhysicsProcessSelected(float delta)
		{
			throw new NotImplementedException();
		}

		public override void PhysicsProcessUnselected(float delta)
		{
			throw new NotImplementedException();
		}

		public override void ProcessSelected(float delta)
		{
			throw new NotImplementedException();
		}

		public override void ProcessUnselected(float delta)
		{
			throw new NotImplementedException();
		}
	}
}
