using Godot;
using System.Threading.Tasks;

namespace Lastdew
{
	public partial class Program : Node
	{
		private Game Game { get; set; }
		
		public override void _Ready()
		{
			MakeNewGame();
		}

		public override void _ExitTree()
		{
			base._ExitTree();

			Game.Ui.MainMenu.Exit.OnToStartMenu -= Reset;
		}

		private void MakeNewGame()
		{
			PackedScene gameScene = GD.Load<PackedScene>(Uids.GAME);
			Game = (Game)gameScene.Instantiate();
			this.AddChildDeferred(Game);
			CallDeferred(MethodName.Subscribe);
		}

		private void Subscribe()
		{
			Game.Ui.MainMenu.Exit.OnToStartMenu += Reset;
		}

		private async Task Reset()
		{
			Game.Ui.MainMenu.Exit.OnToStartMenu -= Reset;
			Game.Fader.FadeOut();
			
			await ToSignal(Game.Fader, Fader.SignalName.OnFadeOut);
			
			Game.QueueFree();
			
			await GetTree().ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			
			MakeNewGame();
			
			await GetTree().ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			
			Game.Fader.FadeIn();
		}
	}
}
