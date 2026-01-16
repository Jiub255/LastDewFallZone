using System;
using Godot;

namespace Lastdew
{
	public class TimeManager(float dayLengthInMinutes, float dayStartHour, ProceduralSkyMaterial skyMaterial)
	{
		public event Action OnNewDay;
		
		private float _currentTime = dayStartHour * 60 * 60; // Convert to seconds

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
		private ProceduralSkyMaterial SkyMaterial { get; set; } = skyMaterial;
		private Curve SunlightCurve { get; } = GD.Load<Curve>("uid://ptuywn8buljw");

		public void Process(float delta)
		{
			if (HomeBase == null)
			{
				return;
			}
			
			CurrentTime += delta * TickRate;
			HomeBase.RotateLight(CurrentTime);
			SetSkyDarkness();
		}

		private void SetSkyDarkness()
		{
			// TODO: 6 am = 21600 s, 6 pm = 64800 s
			// Have the sky lighten from white to black at sunrise over an hour or two? Same for sunset.
			float alpha = SunlightCurve.Sample(CurrentTime);
			SkyMaterial.SkyCoverModulate = new Color(1f, 1f, 1f, alpha);
		}
	}
}
