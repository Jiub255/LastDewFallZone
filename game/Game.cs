using Godot;

namespace Lastdew
{
	public partial class Game : Node
	{
		// TODO: Implement InputManager and GameStateMachine, use them here. Not sure how yet.
		private GameStateMachine GameStateMachine { get; } = new();

		public override void _Input(InputEvent @event)
		{
			base._Input(@event);
			
			GameStateMachine.HandleInput(@event);
		}

		public override void _Process(double delta)
		{
			base._Process(delta);

			GameStateMachine.ProcessState((float)delta);
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
			
			GameStateMachine.PhysicsProcessState((float)delta);
		}
	}
}