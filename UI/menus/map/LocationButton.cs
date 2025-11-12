using Godot;
using System;

namespace Lastdew
{
	public partial class LocationButton : Control
	{
		public event Action<LocationData> OnPressed;
		
		[Export]
		public LocationData Data { get; set; }
		
		private bool ButtonDown { get; set; }
		private bool Moved { get; set; }

		public override void _Process(double delta)
		{
			base._Process(delta);
			
			if (ButtonDown && Input.IsActionJustReleased(InputNames.SELECT))
			{
				ButtonDown = false;
				if (Moved)
				{
					Moved = false;
				}
				else
				{
					OnPressed?.Invoke(Data);
				}
			}
		}

		public override void _GuiInput(InputEvent @event)
		{
			base._GuiInput(@event);
			
			if (!ButtonDown && @event is InputEventMouseButton mouseButton)
			{
				if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
				{
					ButtonDown = true;
				}
			}
			
			if (ButtonDown && @event is InputEventMouseMotion)
			{
				Moved = true;
			}
		}
	}
}
