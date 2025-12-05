using Godot;
using System;

namespace Lastdew
{
	public partial class MainMenu : Menu
	{
		public event Action OnNewGame;
		public event Action OnSaveGame;
		public event Action OnLoadGame;
		
		public SfxButton Continue { get; private set; }
		public SfxButton SaveGame { get; private set;}
		public SfxButton ReturnToBase { get; private set;}
		public SfxButton LoadGame { get; private set;}
		public SfxButton NewGame { get; private set; }
		public SfxButton Options { get; private set; }
		public ExitButton Exit { get; private set; }
		
		public override void _Ready()
		{
			Continue = GetNode<SfxButton>("%Continue");
			SaveGame = GetNode<SfxButton>("%SaveGame");
			ReturnToBase = GetNode<SfxButton>("%ReturnToBase");
			LoadGame = GetNode<SfxButton>("%LoadGame");
			NewGame = GetNode<SfxButton>("%NewGame");
			Options = GetNode<SfxButton>("%Options");
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
