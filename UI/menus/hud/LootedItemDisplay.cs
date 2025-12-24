using Godot;

namespace Lastdew
{
	public partial class LootedItemDisplay : HBoxContainer
	{
		private const string MAIN = "main";

		private AnimationPlayer _animationPlayer;


		public void Setup(Texture2D icon, string name)
		{
			_animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
			TextureRect iconUi = GetNode<TextureRect>("%Icon");
			Label label = GetNode<Label>("%Label");

			_animationPlayer.AnimationFinished += OnAnimationFinished;
			_animationPlayer.Play(MAIN);
			
			iconUi.Texture = icon;
			label.Text = name;
		}

		private void OnAnimationFinished(StringName animationName)
		{
			if (animationName == MAIN)
			{
				_animationPlayer.AnimationFinished -= OnAnimationFinished;
				QueueFree();
			}
		}
	}
}
