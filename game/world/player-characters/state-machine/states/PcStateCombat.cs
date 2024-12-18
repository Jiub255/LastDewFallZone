namespace Lastdew
{	
	public partial class PcStateCombat : PcState
	{
		private Enemy Target { get; set; }
		private int AttackPower { get; } = 1;
		private float AttackDuration { get; } = 1f;
		private float Timer { get; set; }
		
		public PcStateCombat(PcStateContext context) : base(context) {}
	
		public override void EnterState(MovementTarget target)
		{
			if (target.Target is Enemy enemy)
			{
				Target = enemy;
			}
		}

		public override void ProcessSelected(float delta)
		{
			Fight(delta);
		}
	
		public override void ProcessUnselected(float delta)
		{
			Fight(delta);
		}

		public override void PhysicsProcessSelected(float delta) {}
		public override void PhysicsProcessUnselected(float delta) {}
		public override void ExitState() {}
		
		public void HitEnemy()
		{
			// Play enemy get hit animation
			// Take enemy health
			Target.GetHit(AttackPower);
		}
		
		public void GetHit()
		{
			// Play PC get hit animation
			Context.PcAnimationTree.Set("parameters/conditions/GettingHit", true);
		}

		private void Fight(float delta)
		{
			Timer -= delta;
			if (Timer < 0)
			{
				Timer = AttackDuration;
				Attack();
			}
		}
	
		private void Attack()
		{
			// Play PC attack animation
			Context.PcAnimationTree.Set("parameters/conditions/Attacking", true);
		}
	}
}
