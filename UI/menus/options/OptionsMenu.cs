using Godot;

namespace Lastdew
{
	public partial class OptionsMenu : Menu
	{
		public SfxButton Back { get; private set; }
		private HSlider Main { get; set; }
		private HSlider Music { get; set; }
		private HSlider Sfx { get; set; }
		private HSlider Ui { get; set; }
		private HSlider Combat { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			Back = GetNode<SfxButton>("%BackButton");
			Main = GetNode<HSlider>("%MainSlider");
			Music = GetNode<HSlider>("%MusicSlider");
			Sfx = GetNode<HSlider>("%SfxSlider");
			Ui = GetNode<HSlider>("%UiSlider");
			Combat = GetNode<HSlider>("%CombatSlider");

			Main.ValueChanged += SetMainVolume;
			Music.ValueChanged += SetMusicVolume;
			Sfx.ValueChanged += SetSfxVolume;
			Ui.ValueChanged += SetUiVolume;
			Combat.ValueChanged += SetCombatVolume;
			
			Main.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Master"));
			Music.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Music"));
			Sfx.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Sfx"));
			Ui.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Ui"));
			Combat.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Combat"));
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			Main.ValueChanged -= SetMainVolume;
			Music.ValueChanged -= SetMusicVolume;
			Sfx.ValueChanged -= SetSfxVolume;
			Ui.ValueChanged -= SetUiVolume;
			Combat.ValueChanged -= SetCombatVolume;
		}

		private static void SetMainVolume(double value)
		{
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Master"), (float)value);
		}

		private static void SetMusicVolume(double value)
		{
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Music"), (float)value);
		}

		private static void SetSfxVolume(double value)
		{
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Sfx"), (float)value);
		}

		private static void SetUiVolume(double value)
		{
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Ui"), (float)value);
		}

		private static void SetCombatVolume(double value)
		{
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Combat"), (float)value);
		}
	}
}
