using Godot;

namespace Lastdew
{	
	public partial class PcStateCombat : PcState
	{
		private Enemy Target { get; set; }
		private int AttackPower { get; } = 1;
		private float TimeBetweenAttacks { get; } = 2.3f;
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
			this.PrintDebug("HitEnemy called");
			// Play enemy get hit animation
			// Take enemy health
			Target.GetHit(AttackPower);
		}
		
		public void GetHit()
		{
			// Play PC get hit animation
			Context.PcAnimationTree.Set("parameters/conditions/GettingHit", true);
			Context.PcAnimationTree.CallDeferred(AnimationTree.MethodName.Set, "parameters/conditions/GettingHit", false);
			this.PrintDebug("Combat state GetHit called");
		}

		private void Fight(float delta)
		{
			Timer -= delta;
			if (Timer < 0)
			{
				Timer = TimeBetweenAttacks;
				Attack();
			}
		}
	
		private void Attack()
		{
			this.PrintDebug($"Pc attack called");
			// Play PC attack animation
			Context.PcAnimationTree.Set("parameters/conditions/Attacking", true);
			Context.PcAnimationTree.CallDeferred(AnimationTree.MethodName.Set, "parameters/conditions/Attacking", false);
		}
	}
}
