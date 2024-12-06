using Godot;

namespace Lastdew
{
	public class TestTask : ITask
	{
		public TestTask(){}
		
		public void Task(double delta)
		{
			GD.Print("Task completed");
		}
	}
}
