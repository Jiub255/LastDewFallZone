using System;

namespace Lastdew
{
	public class TimeManager(float dayLengthInMinutes)
	{
		public event Action OnNewDay;
		
		// Starting the game at noon -> 43200 seconds
		private const float GAME_START_TIME = 43200;
		
		private float _currentTime = GAME_START_TIME;

		public float CurrentTime
		{
			get => _currentTime;
			private set
			{
				_currentTime = value;
				// Reset each day
				if (_currentTime > 86400)
				{
					_currentTime -= 86400;
					OnNewDay?.Invoke();
				}
			}
		}
		public HomeBase HomeBase { get; set; }
		
		// Game seconds elapsed per real world second
		private float TickRate => 24 * 60 / dayLengthInMinutes;
		
		public void Process(float delta)
		{
			if (HomeBase == null)
			{
				return;
			}
			
			CurrentTime += delta * TickRate;
			HomeBase.RotateLight(CurrentTime);
		}
	}
}
