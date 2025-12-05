using Godot;

namespace Lastdew
{
	public partial class NumberPopup3d : Node3D
	{
		private const string POPUP_ANIMATION_NAME = "popup";
		
		private AnimationPlayer AnimationPlayer { get; set; }
		private Label3D Label { get; set; }

		public override void _Ready()
		{
			AnimationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
			Label = GetNode<Label3D>("%Label3D");

			AnimationPlayer.AnimationFinished += Destroy;
		}

		public void Show(string text)
		{
			Label.Text = text;
			AnimationPlayer.Play(POPUP_ANIMATION_NAME);
		}

		private void Destroy(StringName animationName)
		{
			if (animationName == POPUP_ANIMATION_NAME)
			{
				AnimationPlayer.AnimationFinished -= Destroy;
				QueueFree();
			}
		}
	}
}
