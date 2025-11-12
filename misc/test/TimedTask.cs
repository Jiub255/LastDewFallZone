using System;

namespace Lastdew
{
	public abstract class TimedTask : ITask
	{
		public event Action<TimedTask> OnTaskComplete;
		
		private double _timer;
		
		public TimedTask(double duration)
		{
			_timer = duration;
		}
		
		public void Task(double delta)
		{
			_timer -= delta;
			if (_timer < 0)
			{
				OnTaskComplete?.Invoke(this);
				return;
			}
			ActualTask(delta);
		}

		protected abstract void ActualTask(double delta);
	}
}
