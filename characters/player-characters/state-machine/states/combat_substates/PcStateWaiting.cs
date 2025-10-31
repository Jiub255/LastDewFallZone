namespace Lastdew
{
	public class PcStateWaiting(PlayerCharacter pc) : PcCombatSubstate(pc)
	{
        public override void ProcessSelected(float delta)
		{
			base.ProcessSelected(delta);
			
			Pc.RotateToward(Target.GlobalPosition, TurnSpeed * delta);
		}

		public override void ProcessUnselected(float delta)
		{
			base.ProcessUnselected(delta);
			
			Pc.RotateToward(Target.GlobalPosition, TurnSpeed * delta);
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
				Timer = TimeBetweenAttacks;
				Attack();
			}
		}
	
		private void Attack()
		{
			ChangeSubstate(PcCombatSubstateNames.ATTACKING);
		}
	}
}
