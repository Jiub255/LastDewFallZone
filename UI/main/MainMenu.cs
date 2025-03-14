using Godot;
using System;

namespace Lastdew
{
	public partial class MainMenu : Menu
	{
		public event Action OnNewGame;
		public event Action OnSaveGame;
		public event Action OnLoadGame;
		
		public Button Continue { get; private set; }
		public Button SaveGame { get; private set;}
		public Button ReturnToBase { get; private set;}
		public Button LoadGame { get; private set;}
		public Button NewGame { get; private set; }
		public Button Options { get; private set; }
		public ExitButton Exit { get; private set; }
		
		public override void _Ready()
		{
			Continue = GetNode<Button>("%Continue");
			SaveGame = GetNode<Button>("%SaveGame");
			ReturnToBase = GetNode<Button>("%ReturnToBase");
			LoadGame = GetNode<Button>("%LoadGame");
			NewGame = GetNode<Button>("%NewGame");
			Options = GetNode<Button>("%Options");
			Exit = GetNode<ExitButton>("%Exit");

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
