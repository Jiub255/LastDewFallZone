namespace Lastdew
{
	public class TimeManager(float dayLengthInMinutes)
	{
		// Starting the game at noon -> 43200 seconds
		private float _currentTime = 43200;

		public float CurrentTime
		{
			get => _currentTime;
			private set
			{
				_currentTime = value;
				// Reset each day to avoid overflow
				if (_currentTime > 86400)
				{
					_currentTime -= 86400;
				}
			}
		}

		/// <summary>
		/// Game seconds elapsed per real world second
		/// </summary>
		private float TickRate => 24 * 60 / dayLengthInMinutes;

		private HomeBase HomeBase { get; set; }

		public void Initialize(HomeBase homeBase)
		{
			HomeBase = homeBase;
		}
		
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
