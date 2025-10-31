
namespace Lastdew
{	public class PcStateIdle(PlayerCharacter pc) : PcState(pc)
	{
		public override void EnterState()
		{
			pc.MovementTarget = new MovementTarget();
		}
		public override void ExitState() {}
		public override void PhysicsProcessSelected(float delta) {}
		public override void PhysicsProcessUnselected(float delta) {}
		public override void ProcessSelected(float delta) {}
		
		// TODO: Look for enemies and/or loot containers within a certain radius? Or just find the closest
		// and go for it? OR, choose per character what to do. Like have weaker chars auto target loot and 
		// stronger ones target enemies. 
		public override void ProcessUnselected(float delta)
		{
		}
	}
}
