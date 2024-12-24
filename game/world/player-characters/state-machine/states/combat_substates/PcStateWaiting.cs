namespace Lastdew
{
	public partial class PcStateWaiting : PcCombatSubstate
	{
		public PcStateWaiting(PcAnimationTree pcAnimationTree) : base(pcAnimationTree) {}

		public override void EnterState(Enemy target) {}

		public override void ExitState() {}

		public override void GetHit(Enemy attacker)
		{
			ChangeSubstate(PcCombatSubstateNames.GETTING_HIT, attacker);
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
			ChangeSubstate(PcCombatSubstateNames.ATTACKING, Target);
		}
	}
}
