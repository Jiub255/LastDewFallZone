using Godot;

namespace Lastdew
{
    public class EnemyStateMovement(Enemy enemy) : EnemyState(enemy)
    {
        private const float MAX_SPEED = 4f;
        private const float ACCELERATION = 35f;
        
        public override void EnterState()
        {
            Enemy.LastTargetPosition = GetAttackPosition(Enemy.Target);
            Enemy.NavigationAgent.TargetPosition = GetAttackPosition(Enemy.Target);
        }

        public override void ProcessState(float delta)
        {
            if (TargetDead())
            {
                ChangeState(EnemyStateNames.IDLE);
                return;
            }
            if (WithinRangeOfEnemy())
            {
                ChangeState(EnemyStateNames.COMBAT);
                return;
            }
            Animate();
            Vector3 nextPosition = Enemy.NavigationAgent.GetNextPathPosition();
            Accelerate(delta, nextPosition);
            Enemy.RotateToward(nextPosition, Enemy.TURN_SPEED * delta);
            RecalculateTargetPositionIfTargetMovedEnough(delta);
        }

        private void Animate()
        {
            float blendAmount = Mathf.Clamp(Enemy.Velocity.Length() / MAX_SPEED, 0, 1);
            Enemy.SetBlendAmount(blendAmount);
        }

        private void Accelerate(float delta, Vector3 nextPosition)
        {
            Vector3 direction = (nextPosition - Enemy.GlobalPosition).Normalized();
            Vector3 targetVelocity = direction * MAX_SPEED;
            float accelerationAmount = ACCELERATION * delta;
            Enemy.Velocity = Enemy.Velocity.MoveToward(targetVelocity, accelerationAmount);
            Enemy.MoveAndSlide();
        }
        
        private bool WithinRangeOfEnemy()
        {
            return Enemy.NavigationAgent.IsNavigationFinished();
        }
    }
}
