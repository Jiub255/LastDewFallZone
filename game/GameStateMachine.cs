using Godot;

namespace Lastdew
{
	public class GameStateMachine
	{
		private GameState CurrentState { get; set; }
		
		public GameStateMachine()
		{

		}
		
		public void HandleInput(InputEvent @event)
		{
			CurrentState.HandleInput(@event);
		}
		
		public void ProcessState(float delta)
		{
			CurrentState.ProcessState(delta);
		}
		
		public void PhysicsProcessState(float delta)
		{
			CurrentState.PhysicsProcessState(delta);
		}
	}
}
