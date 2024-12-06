using Godot;

namespace Lastdew
{
	public class TestTimedTask : TimedTask
	{
        public TestTimedTask(double duration) : base(duration)
        {
        }

        protected override void ActualTask(double delta)
		{
			GD.Print("Timed task");
		}
	}
}
