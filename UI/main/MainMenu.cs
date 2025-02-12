using Godot;
using System;

namespace Lastdew
{
	public partial class MainMenu : Menu
	{
		public event Action OnNewGame;
		public event Action OnSaveGame;
		public event Action OnLoadGame;
		
		private Button NewGame { get; set; }
		private Button SaveGame { get; set;}
		private Button LoadGame { get; set;}
		
		public override void _Ready()
		{
			NewGame = GetNode<Button>("%NewGame");
			SaveGame = GetNode<Button>("%SaveGame");
			LoadGame = GetNode<Button>("%LoadGame");

			NewGame.Pressed += StartNewGame;
			SaveGame.Pressed += Save;
			LoadGame.Pressed += Load;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			NewGame.Pressed -= StartNewGame;
			SaveGame.Pressed -= Save;
			LoadGame.Pressed -= Load;
		}

		private void StartNewGame()
		{
			OnNewGame?.Invoke();
		}
		
		private void Save()
		{
			OnSaveGame?.Invoke();
		}
		
		private void Load()
		{
			OnLoadGame?.Invoke();
		}
	}
}
