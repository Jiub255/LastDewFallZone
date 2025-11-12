using Godot;
using System;
using System.Collections.Generic;

namespace Lastdew
{
	public class TaskDoer
	{
		public event Action<double> OnActions;
		public List<ITask> Tasks = new();

		public TaskDoer(ITask[] tasks)
		{
			foreach (ITask task in tasks)
			{
				AddTask(task);
				if (task is TimedTask timedTask)
				{
					timedTask.OnTaskComplete += RemoveTimedTask;
				}
			}
		}
		
		public void Process(double delta)
		{
			OnActions?.Invoke(delta);
		}
		
		public void Exit()
		{
			foreach (ITask task in Tasks)
			{
				if (task is TimedTask timedTask)
				{
					timedTask.OnTaskComplete -= RemoveTimedTask;
				}
				RemoveTask(task);
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
			GD.Print("Timed task removed");
		}
	}
}
