using System;
using Godot;

namespace Lastdew
{
	public partial class PcButton : Control
	{
		public event Action<PlayerCharacter> OnSelectPc;
		public event Action<PlayerCharacter> OnCenterOnPc;
	
		private const float PRESS_PITCH = 0.8f;
		private const float VOLUME = -10f;
		private const double DOUBLE_CLICK_TIME = 0.5d;

		private double _timer;
		private bool _recentlyClicked;
		
		private TextureRect PcIcon { get; set; }
		private RichTextLabel PcName { get; set; }
		private ProgressBar PainBar { get; set; }
		private ProgressBar InjuryBar { get; set; }
		private PlayerCharacter Pc { get; set; }

		private AudioStreamPlayer AudioStreamPlayer { get; } = new();
		private AudioStream Click { get; } = GD.Load<AudioStream>(Sfx.CLICK_3);

		
		public override void _Ready()
		{
			base._Ready();
	        
			CallDeferred(Node.MethodName.AddChild, AudioStreamPlayer);
			AudioStreamPlayer.Stream = Click;
			AudioStreamPlayer.VolumeDb = VOLUME;
			AudioStreamPlayer.PitchScale = PRESS_PITCH;
		}

		public override void _Process(double delta)
		{
			base._Process(delta);

			if (!_recentlyClicked)
			{
				return;
			}
			
			_timer += delta;
			if (_timer >= DOUBLE_CLICK_TIME)
			{
				_recentlyClicked = false;
				_timer = 0;
			}
		}
		
		public override void _ExitTree()
		{
			base._ExitTree();
			
			Pc.StatManager.Health.OnHealthChanged -= SetHealthBars;
		}

		public override void _GuiInput(InputEvent @event)
		{
			base._GuiInput(@event);

			if (@event.IsLeftClick())
			{
				AudioStreamPlayer.Play();
				if (_recentlyClicked)
				{
					// TODO: Center camera on PC.
					OnCenterOnPc?.Invoke(Pc);
					_recentlyClicked = false;
					_timer = 0;
				}
				else
				{
					OnSelectPc?.Invoke(Pc);
					_recentlyClicked = true;
				}
				GetViewport().SetInputAsHandled();
			}
		}

		// Called in a "call deferred" from another script (HUD). Still needs to be public?
		public void Setup(PlayerCharacter pc)
		{
			PcIcon = GetNode<TextureRect>("%Icon");
			PcName = GetNode<RichTextLabel>("%Name");
			PainBar = GetNode<ProgressBar>("%PainBar");
			InjuryBar = GetNode<ProgressBar>("%InjuryBar");
			
			PcIcon.Texture = pc.Data.Icon;
			PcName.Text = pc.Data.Name;
			Pc = pc;
			
			SetHealthBars();
			
			Pc.StatManager.Health.OnHealthChanged += SetHealthBars;
		}
		
		private void SetHealthBars(int _)
		{
			SetHealthBars();
		}
		
		public void SetHealthBars()
		{
			PainBar.Value = Pc.StatManager.Health.Pain;
			InjuryBar.Value = Pc.StatManager.Health.Injury;
		}
	}
}
