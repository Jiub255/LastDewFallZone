namespace Lastdew
{
	public partial class PcStateWaiting(PcStateContext context) : PcCombatSubstate(context)
	{
        public override void ProcessSelected(float delta)
		{
			base.ProcessSelected(delta);
			
			Context.RotateToward(Target.GlobalPosition, TurnSpeed * delta);
		}

		public override void ProcessUnselected(float delta)
		{
			base.ProcessUnselected(delta);
			
			Context.RotateToward(Target.GlobalPosition, TurnSpeed * delta);
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
