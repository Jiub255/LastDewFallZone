using Godot;

namespace Lastdew
{
    public class EnemyStateMovement(Enemy enemy) : EnemyState(enemy)
    {
        private const string MOVEMENT_BLEND_TREE_NAME = "movement_blend_tree";
        
        public override void EnterState()
        {
            Enemy.AnimStateMachine.Travel(MOVEMENT_BLEND_TREE_NAME);
            Enemy.LastTargetPosition = GetAttackPosition(Enemy.Target);
            Enemy.NavigationAgent.TargetPosition = GetAttackPosition(Enemy.Target);
        }

        public override void ProcessState(float delta)
        {
            if (WithinRangeOfEnemy())
            {
                ChangeState(EnemyStateNames.COMBAT);
                return;
            }
            Animate();
            Vector3 nextPosition = Enemy.NavigationAgent.GetNextPathPosition();
            Accelerate(delta, nextPosition);
            Enemy.RotateToward(nextPosition, Enemy.Data.TurnSpeed * delta);
            RecalculateTargetPositionIfTargetMovedEnough(delta);
        }

        private void Animate()
        {
            float blendAmount = Mathf.Clamp(Enemy.Velocity.Length() / Enemy.Data.MaxSpeed, 0, 1);
            Enemy.SetBlendAmount(blendAmount);
        }

        private void Accelerate(float delta, Vector3 nextPosition)
        {
            Vector3 direction = (nextPosition - Enemy.GlobalPosition).Normalized();
            Vector3 targetVelocity = direction * Enemy.Data.MaxSpeed;
            float accelerationAmount = Enemy.Data.Acceleration * delta;
            Enemy.Velocity = Enemy.Velocity.MoveToward(targetVelocity, accelerationAmount);
            Enemy.MoveAndSlide();
        }
        
        private bool WithinRangeOfEnemy()
        {
            return Enemy.NavigationAgent.IsNavigationFinished();
        }
    }
}
