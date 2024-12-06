using Godot;
using System;

namespace Lastdew
{
	public partial class TestTaskDoer2 : Node
	{
		private TaskDoer _taskDoer;

		public override void _Ready()
		{
			base._Ready();
			
			
			TestTask testTask = new TestTask();
			TestTimedTask timedTask = new TestTimedTask(0.5f);
			_taskDoer = new TaskDoer(new ITask[] { testTask, timedTask });
		}

		public override void _Process(double delta)
		{
			base._Process(delta);
			
			_taskDoer.Process(delta);
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			_taskDoer.Exit();
		}
	}
}
