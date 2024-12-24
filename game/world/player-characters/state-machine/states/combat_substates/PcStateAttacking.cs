using Godot;

namespace Lastdew
{
	public partial class PcStateAttacking : PcCombatSubstate
	{
		private int AttackPower { get; } = 1;
		
		public PcStateAttacking(PcAnimationTree pcAnimationTree) : base(pcAnimationTree)
		{
			pcAnimationTree.Connect(
				AnimationTree.SignalName.AnimationFinished,
				Callable.From((string animationName) => OnAnimationFinished(animationName)));
		}

		public override void EnterState(Enemy target)
		{
			base.EnterState(target);

			// TODO: Use advance expressions instead of conditions, and put Attacking and GettingHit bools in
			// PcAnimationTree? These don't seem to be working, probably misunderstanding somehting
			PcAnimationTree.Attacking = true;
			//PcAnimationTree.Set("parameters/conditions/Attacking", true);
		}

		public override void ExitState()
		{
			PcAnimationTree.Attacking = false;
			//PcAnimationTree.Set("parameters/conditions/Attacking", false);
		}

		public override void GetHit(Enemy attacker)
		{
			ChangeSubstate(PcCombatSubstateNames.GETTING_HIT, attacker);
		}
		
		public void HitEnemy()
		{
			Target.GetHit(AttackPower);
		}
		
		private void OnAnimationFinished(string animationName)
		{
			this.PrintDebug($"{animationName} finished.");
			if (animationName == "Punch_Right")
			{
				ChangeSubstate(PcCombatSubstateNames.WAITING, Target);
			}
		}
	}
}
