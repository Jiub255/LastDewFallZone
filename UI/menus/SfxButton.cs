using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class SfxButton : Button
	{
		private const float PRESS_PITCH = 0.8f;
		private const float RELEASE_PITCH = 1.1f;
		private const float VOLUME = -10f;
		
		private AudioStreamPlayer AudioStreamPlayer { get; } = new();
		private AudioStream Click { get; } = GD.Load<AudioStream>(Sfx.CLICK_3);
	
	    public override void _Ready()
	    {
	        base._Ready();
	        
	        this.AddChildDeferred(AudioStreamPlayer);
		    AudioStreamPlayer.Stream = Click;
		    AudioStreamPlayer.VolumeDb = VOLUME;

		    ButtonDown += PlayPressSound;
		    ButtonUp += PlayReleaseSound;
	    }

	    public override void _ExitTree()
	    {
		    base._ExitTree();
		    
		    ButtonDown -= PlayPressSound;
		    ButtonUp -= PlayReleaseSound;
	    }

	    private void PlayPressSound()
	    {
		    PlayClickSound(PRESS_PITCH);
	    }

	    private void PlayReleaseSound()
	    {
		    PlayClickSound(RELEASE_PITCH);
	    }
	    
	    private void PlayClickSound(float pitch)
	    {
		    AudioStreamPlayer.PitchScale = pitch;
		    AudioStreamPlayer.Play();
	    }
	}
}
