using System;

namespace Lastdew
{
	public abstract class EnemyState(Enemy enemy)
	{
		public event Action<EnemyStateNames> OnChangeState;
		
		public Enemy Enemy { get; } = enemy;

		public abstract void EnterState();
		public abstract void ExitState();
		public abstract void ProcessState(float delta);
		public abstract void PhysicsProcessState(float delta);

		public void ChangeState(EnemyStateNames newStateName)
		{
			OnChangeState?.Invoke(newStateName);
		}
	}
}
