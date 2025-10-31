using Godot;

namespace Lastdew
{
	public class PcStateGettingHit : PcCombatSubstate
	{
		private const string GETTING_HIT_ANIM_NAME = "CharacterArmature|HitRecieve";
		
		public PcStateGettingHit(PlayerCharacter pc) : base(pc)
		{
			pc.PcAnimationTree.Connect(
				AnimationMixer.SignalName.AnimationFinished,
				Callable.From((string animationName) => OnAnimationFinished(animationName)));
		}

		public override void EnterState(Enemy target)
		{
			base.EnterState(target);
			
			Pc.AnimStateMachine.Travel(GETTING_HIT_ANIM_NAME);
		}
 
		public override void GetHit()
		{
			ChangeSubstate(PcCombatSubstateNames.GETTING_HIT);
		}
		
		// TODO: This still getting called from the AnimationTree signal, even if the pc died
		// and is in incapacitated state. Might be fixed with Context.Incapacitated?
		private void OnAnimationFinished(string animationName)
		{
			if (animationName == GETTING_HIT_ANIM_NAME && !Pc.Incapacitated)
			{
				ChangeSubstate(PcCombatSubstateNames.WAITING);
			}
		}
	}
}
