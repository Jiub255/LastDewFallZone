using Godot;

namespace Lastdew
{
	public partial class PcStateGettingHit : PcCombatSubstate
	{
		public PcStateGettingHit(PcAnimationTree pcAnimationTree) : base(pcAnimationTree)
		{
			pcAnimationTree.Connect(
				AnimationTree.SignalName.AnimationFinished,
				Callable.From((string animationName) => OnAnimationFinished(animationName)));
		}

		public override void EnterState(Enemy target)
		{
			base.EnterState(target);
			
			PcAnimationTree.GettingHit = true;
			//PcAnimationTree.Set("parameters/conditions/GettingHit", true);
		}

		public override void ExitState()
		{
			PcAnimationTree.GettingHit = false;
			//PcAnimationTree.Set("parameters/conditions/GettingHit", false);
		}

        public override void GetHit(Enemy attacker) {}
		
		private void OnAnimationFinished(string animationName)
		{
			if (animationName == "HitRecieve")
			{
				ChangeSubstate(PcCombatSubstateNames.WAITING, Target);
			}
		}
	}
}
