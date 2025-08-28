using System;

namespace Lastdew
{
	public class Timer
	{
		private double _timer;
		private float _duration;
		private Action _action;
		private bool _repeating;
		private bool _done;
		
		public Timer(Action action, float duration, bool repeating)
		{
			_timer = duration;
			_duration = duration;
			_action = action;
			_repeating = repeating;
		}
		
		public void Tick(double delta)
		{
			if (_done)
			{
				return;
			}
			_timer -= delta;
			if (_timer > 0)
			{
				return;
			}
			_action?.Invoke();
			if (_repeating)
			{
				_timer = _duration;
			}
			else
			{
				_done = true;
			}
		}
	}
}
