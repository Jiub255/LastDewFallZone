using Godot;
using System;

namespace Lastdew
{
	public partial class MainMenu : Menu
	{
		public event Action<PackedScene> OnNewGame;
		
		private Button NewGame { get; set; }
		private PackedScene HomeBaseScene { get; } = GD.Load<PackedScene>("res://game/world/home-test-env.tscn");
		
		public override void _Ready()
		{
			NewGame = GetNode<Button>("%NewGame");

			NewGame.Pressed += StartNewGame;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			NewGame.Pressed -= StartNewGame;
		}

		private void StartNewGame()
		{
			OnNewGame?.Invoke(HomeBaseScene);
		}
	}
}
