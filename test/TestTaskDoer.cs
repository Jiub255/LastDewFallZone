using Godot;
using System;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class TestTaskDoer : Node
	{
		public event Action<double> OnActions;
		public List<ITask> Tasks = new();

		private double Timer { get; set; } = 1f;

		public override void _Ready()
		{
			base._Ready();
			
			TestTask testTask = new TestTask();
			AddTask(testTask);
			
			TestTimedTask testTimedTask = new TestTimedTask(1f);
			testTimedTask.OnTaskComplete += RemoveTimedTask;
			AddTask(testTimedTask);
		}
		
		public override void _Process(double delta)
		{
			base._Process(delta);

			OnActions?.Invoke(delta);
			
			
			Timer -= delta;
			GD.Print($"Time: {Timer}");
			if (Timer < 0 && Tasks.Count > 0)
			{
				RemoveTask(Tasks[0]);
			}
		}
		
		private void AddTask(ITask task)
		{
			Tasks.Add(task);
			OnActions += task.Task;
		}
		
		private void RemoveTask(ITask task)
		{
			Tasks.Remove(task);
			OnActions -= task.Task;
		}
		
		private void RemoveTimedTask(TimedTask task)
		{
			Tasks.Remove(task);
			OnActions -= task.Task;
			task.OnTaskComplete -= RemoveTimedTask;
		}
	}
}
