using Godot;

namespace Lastdew
{
	public partial class Clock : Label
	{
		private TimeManager TimeManager { get; set; }

		public override void _Process(double delta)
		{
			base._Process(delta);
			
			UpdateTime();
		}

		public void Initialize(TimeManager timeManager)
		{
			TimeManager = timeManager;
		}

		private void UpdateTime()
		{
			if (TimeManager == null)
			{
				return;
			}
			
			Text = TimeManager.CurrentTime.SecondsToClockTime();
		}
	}
}
