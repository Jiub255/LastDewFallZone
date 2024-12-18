using Godot;

namespace Lastdew
{
	public abstract class GameState
	{
		public abstract void HandleInput(InputEvent @event);
		public abstract void ProcessState(float delta);
		public abstract void PhysicsProcessState(float delta);
	}
}
