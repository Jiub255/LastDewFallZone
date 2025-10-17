using Godot;

namespace Lastdew
{
	public partial class PcStateGettingHit : PcCombatSubstate
	{
		private const string GETTING_HIT_ANIM_NAME = "CharacterArmature|HitRecieve";
		
		public PcStateGettingHit(PcStateContext context) : base(context)
		{
			context.PcAnimationTree.Connect(
				AnimationMixer.SignalName.AnimationFinished,
				Callable.From((string animationName) => OnAnimationFinished(animationName)));
		}

		public override void EnterState(Enemy target)
		{
			base.EnterState(target);
			
			Context.AnimStateMachine.Travel(GETTING_HIT_ANIM_NAME);
		}
 
		public override void GetHit()
		{
			ChangeSubstate(PcCombatSubstateNames.GETTING_HIT);
		}
		
		// TODO: This still getting called from the AnimationTree signal, even if the pc died
		// and is in incapacitated state. Might be fixed with Context.Incapacitated?
		private void OnAnimationFinished(string animationName)
		{
			if (animationName == GETTING_HIT_ANIM_NAME && !Context.Incapacitated)
			{
				ChangeSubstate(PcCombatSubstateNames.WAITING);
			}
		}
	}
}
