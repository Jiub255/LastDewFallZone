namespace Lastdew
{
    public class EnemyStateCombat(Enemy enemy) : EnemyState(enemy)
    {
        private const string ATTACK_ANIM_NAME = "CharacterArmature|Punch_Right";
		private const float A_LITTLE_BIT = 1f;
        private float AttackTimer { get; set; }

        public override void EnterState()
        {
            Enemy.SetBlendAmount(0f);
        }

        public override void ProcessState(float delta)
        {
            if (OutOfRangeOfEnemy())
            {
                ChangeState(EnemyStateNames.MOVEMENT);
                return;
            }

            Enemy.RotateToward(Enemy.Target.Pc.GlobalPosition, Enemy.Data.TurnSpeed * delta);
			
            AttackTimer -= delta;
            // TODO: Make sure not in "get hit" animation before starting attack.
            // Probably just introduce "GETTING_HIT" state.
            if (AttackTimer < 0)
            {
                AttackTimer = Enemy.Data.TimeBetweenAttacks;
				
                // Attack animation (Calls Enemy.HitTarget() from animation track)
                Enemy.AnimStateMachine.Travel(ATTACK_ANIM_NAME);
            }
            RecalculateTargetPositionIfTargetMovedEnough(delta);
        }

        private bool OutOfRangeOfEnemy()
        {
            float distanceSquared = Enemy.GlobalPosition.DistanceSquaredTo(GetAttackPosition(Enemy.Target));
            return distanceSquared > A_LITTLE_BIT;
        }
    }
}
