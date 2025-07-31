namespace Lastdew
{
	public class PcStateIncapacitated(PcStateContext context) : PcCombatSubstate(context)
	{
		private const string DEATH_ANIM_NAME = "CharacterArmature|Death";

        public override void EnterState(Enemy target)
		{
			base.EnterState(target);

			Context.AnimStateMachine.Travel(DEATH_ANIM_NAME);
			Context.DisablePC();
		}

		public override void GetHit(){}
	}
}
