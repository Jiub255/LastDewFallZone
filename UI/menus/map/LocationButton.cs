using Godot;
using System;

namespace Lastdew
{
	public partial class LocationButton : Control
	{
		public event Action<LocationData> OnPressed;

		private const float PRESS_PITCH = 0.8f;
		private const float RELEASE_PITCH = 1.1f;
		private const float VOLUME = -10f;
		
		[Export]
		public LocationData Data { get; set; }
		
		private AudioStreamPlayer AudioStreamPlayer { get; } = new();
		private AudioStream Click { get; } = GD.Load<AudioStream>(Sfx.CLICK_3);
		
		private bool ButtonDown { get; set; }
		private bool Moved { get; set; }
		
		
		public override void _Ready()
		{
			base._Ready();
	        
			CallDeferred(Node.MethodName.AddChild, AudioStreamPlayer);
			AudioStreamPlayer.Stream = Click;
		    AudioStreamPlayer.VolumeDb = VOLUME;
		}
		
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
					PlayClickSound(RELEASE_PITCH);
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
					PlayClickSound(PRESS_PITCH);
					ButtonDown = true;
				}
			}
			
			if (ButtonDown && @event is InputEventMouseMotion)
			{
				Moved = true;
			}
		}
		
		private void PlayClickSound(float pitch)
		{
			AudioStreamPlayer.PitchScale = pitch;
			AudioStreamPlayer.Play();
		}
	}
}
