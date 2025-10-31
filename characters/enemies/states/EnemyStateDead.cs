namespace Lastdew
{
	public class EnemyStateDead(Enemy enemy) : EnemyState(enemy)
	{
		private const string DEATH_ANIM_NAME = "CharacterArmature|Death";
		
		public override void EnterState()
		{
			Enemy.AnimStateMachine.Travel(DEATH_ANIM_NAME);
			// TODO: Drop loot? Timer to disappear body?
			Enemy.CollisionLayer = 0;
			Enemy.NavigationAgent.AvoidanceEnabled = false;
		}

		public override void ProcessState(float delta)
		{
		}
	}
}
