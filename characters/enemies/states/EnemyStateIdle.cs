namespace Lastdew
{
	public class EnemyStateIdle(Enemy enemy) : EnemyState(enemy)
	{
		public override void EnterState()
		{
		}

		public override void ExitState()
		{
		}

		public override void ProcessState(float delta)
		{
			// TODO: Look for a new target every second or so.
			// Then switch to movement once one is found.
		}

		public override void PhysicsProcessState(float delta)
		{
		}
	}
}
