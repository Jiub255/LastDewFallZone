using System;
using Godot;

namespace Lastdew
{
	public abstract class EnemyState(Enemy enemy)
	{
		// TODO: Keep static EnemyTarget here instead of on Enemy?
		public event Action<EnemyStateNames> OnChangeState;
		
        private const float RECALCULATION_DISTANCE_SQUARED = 0.25f;
        private const float ATTACK_RADIUS = 1f;
        private const float TIME_BETWEEN_CHECKS = 0.25f;
        
        private float Timer {get; set;}
        
		protected Enemy Enemy { get; } = enemy;

		public abstract void EnterState();
		public abstract void ProcessState(float delta);

		protected void ChangeState(EnemyStateNames newStateName)
		{
			OnChangeState?.Invoke(newStateName);
		}
		
		protected static Vector3 GetAttackPosition(EnemyTarget target)
		{
			return target.Pc.GlobalPosition + target.CombatDirection * ATTACK_RADIUS;
		}
		
		protected void RecalculateTargetPositionIfTargetMovedEnough(float delta)
		{
			Timer += delta;
			if (Timer >= TIME_BETWEEN_CHECKS)
			{
				Timer = 0;
				Vector3 attackPosition = GetAttackPosition(Enemy.Target);
				bool targetMovedEnough = attackPosition.DistanceSquaredTo(Enemy.LastTargetPosition) > RECALCULATION_DISTANCE_SQUARED;
				if (targetMovedEnough)
				{
					Enemy.NavigationAgent.TargetPosition = attackPosition;
					Enemy.LastTargetPosition = attackPosition;
				}
			}
		}
	}
}
