namespace Lastdew
{
	public class PcStateWaiting(PlayerCharacter pc) : PcCombatSubstate(pc)
	{
		private const float UNARMED_TIME_BETWEEN_ATTACKS = 1f;
		
        public override void ProcessSelected(float delta)
		{
			base.ProcessSelected(delta);
			
			Pc.RotateToward(Pc.MovementTarget.Target.GlobalPosition, TurnSpeed * delta);
		}

		public override void ProcessUnselected(float delta)
		{
			base.ProcessUnselected(delta);
			
			Pc.RotateToward(Pc.MovementTarget.Target.GlobalPosition, TurnSpeed * delta);
		}
		
		public override void GetHit()
		{
			ChangeSubstate(PcCombatSubstateNames.GETTING_HIT);
		}

		protected override void Tick(float delta)
		{
			base.Tick(delta);
			
			if (Timer < 0)
			{
				Timer = Pc.Equipment.Weapon?.TimeBetweenAttacks ?? UNARMED_TIME_BETWEEN_ATTACKS;
				Attack();
			}
		}
	
		private void Attack()
		{
			ChangeSubstate(PcCombatSubstateNames.ATTACKING);
		}
	}
}
