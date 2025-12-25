using Godot;

namespace Lastdew
{
	public partial class OptionsMenu : Menu
	{
		public SfxButton Back { get; private set; }
		private HSlider Main { get; set; }
		private HSlider Music { get; set; }
		private HSlider Sfx { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			Back = GetNode<SfxButton>("%BackButton");
			Main = GetNode<HSlider>("%MainSlider");
			Music = GetNode<HSlider>("%MusicSlider");
			Sfx = GetNode<HSlider>("%SfxSlider");

			Main.ValueChanged += SetMainVolume;
			Music.ValueChanged += SetMusicVolume;
			Sfx.ValueChanged += SetSfxVolume;
			
			
			Main.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Master"));
			Music.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Music"));
			Sfx.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Sfx"));
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			Main.ValueChanged -= SetMainVolume;
			Music.ValueChanged -= SetMusicVolume;
			Sfx.ValueChanged -= SetSfxVolume;
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
	}
}
