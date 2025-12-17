using Godot;

namespace Lastdew
{
	public partial class LootedItemDisplay : HBoxContainer
	{
		private const string MAIN = "main";

		private AnimationPlayer _animationPlayer;

		public void Setup(Item item)
		{
			_animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
			TextureRect icon = GetNode<TextureRect>("%Icon");
			Label label = GetNode<Label>("%Label");

			_animationPlayer.AnimationFinished += OnAnimationFinished;
			_animationPlayer.Play(MAIN);
			
			icon.Texture = item.Icon;
			label.Text = item.Name;
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
