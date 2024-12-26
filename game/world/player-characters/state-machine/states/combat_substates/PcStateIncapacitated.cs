namespace Lastdew
{
	public class PcStateIncapacitated : PcCombatSubstate
	{
		private const string DEATH_ANIM_NAME = "CharacterArmature|Death";
		
		public PcStateIncapacitated(PcStateContext context) : base(context) {}
		
		public override void EnterState(Enemy target)
		{
			base.EnterState(target);

			Context.AnimStateMachine.Travel(DEATH_ANIM_NAME);
			Context.DisablePC();
		}

		public override void GetHit(){}
	}
}
