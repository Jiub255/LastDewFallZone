namespace Lastdew
{
	public class PcStateIncapacitated(PlayerCharacter pc) : PcCombatSubstate(pc)
	{
		private const string DEATH_ANIM_NAME = "CharacterArmature|Death";

        public override void EnterState()
		{
			base.EnterState();

			Pc.AnimStateMachine.Travel(DEATH_ANIM_NAME);
			Pc.DisablePc();
		}

		public override void GetHit(){}
	}
}
