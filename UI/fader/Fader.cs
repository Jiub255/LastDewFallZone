using Godot;

namespace Lastdew
{
	public partial class Fader : CanvasLayer
	{
		[Signal]
		public delegate void OnFadeOutEventHandler();
		
		private const string FADE_IN = "fade_in";
		private const string FADE_OUT = "fade_out";
		
		private AnimationPlayer AnimationPlayer { get; set; }

		public override void _Ready()
		{
			base._Ready();

			Hide();
			AnimationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
			AnimationPlayer.AnimationFinished += OnAnimationFinished;
		}
		
		public override void _ExitTree()
		{
			base._ExitTree();
			
			AnimationPlayer.AnimationFinished -= OnAnimationFinished;
		}

		public void FadeOut()
		{
			Show();
			AnimationPlayer.Play(FADE_OUT);
		}

		public void FadeIn()
		{
			AnimationPlayer.Play(FADE_IN);
		}

		private void OnAnimationFinished(StringName animationName)
		{
			if (animationName == FADE_OUT)
			{
				EmitSignal(SignalName.OnFadeOut);
			}
			else if (animationName == FADE_IN)
			{
				Hide();
			}
		}
	}
}
