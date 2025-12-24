using Godot;
using System;

namespace Lastdew
{
	public class TimeManager
	{
		/// <summary>
		/// Game seconds elapsed per real world second
		/// </summary>
		[Export]
		private float TickRate { get; } = 100;

		private float CurrentTime { get; set; }
		private HomeBase HomeBase { get; set; }

		public void Process(double delta)
		{
			CurrentTime += (float)delta * TickRate;
		}
	}
}
