using Godot;

namespace Lastdew
{
	public partial class PcStateGettingHit : PcCombatSubstate
	{
		private const string GETTING_HIT_ANIM_NAME = "CharacterArmature|HitRecieve";
		private AnimationNodeStateMachinePlayback StateMachine { get; set; }
		
		public PcStateGettingHit(PcStateContext context) : base(context)
		{
			context.PcAnimationTree.Connect(
				AnimationTree.SignalName.AnimationFinished,
				Callable.From((string animationName) => OnAnimationFinished(animationName)));
			StateMachine = (AnimationNodeStateMachinePlayback)PcAnimationTree.Get("parameters/playback");
		}

		public override void EnterState(Enemy target)
		{
			base.EnterState(target);
			
			StateMachine.Travel(GETTING_HIT_ANIM_NAME);
		}

		public override void GetHit(Enemy attacker) {}
		
		private void OnAnimationFinished(string animationName)
		{
			if (animationName == GETTING_HIT_ANIM_NAME)
			{
				ChangeSubstate(PcCombatSubstateNames.WAITING, Target);
			}
		}
	}
}
