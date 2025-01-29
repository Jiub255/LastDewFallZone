using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class World : Node3D
	{
		private ClickHandler ClickHandler { get; set; }
		private PcManagerBase PcManager { get; set; }
		private UiManager UI { get; set; }
		private InventoryManager InventoryManager { get; set; }
		private TeamData TeamData { get; set; }
		private SaverLoader SaverLoader { get; set; }
		private PackedScene HomeBaseScene { get; } = GD.Load<PackedScene>("res://game/world/home-test-env.tscn");
		private AllPcScenes AllPcs { get; } = GD.Load<AllPcScenes>("res://game/world/player-characters/management/all_pc_scenes.tres");
		
		public override void _Ready()
		{
			base._Ready();

			Camera camera = GetNode<Camera>("%CameraRig");
			ClickHandler = camera.ClickHandler;
			PcManager = GetNode<PcManagerBase>("%PcManager");
			UI = GetNode<UiManager>("%UiManager");
			SaverLoader = new SaverLoader();
			TeamData = new TeamData(AllPcs, new List<int>() { 0, 1, });
			InventoryManager = new InventoryManager();

			GetTree().Paused = true;

			Subscribe();
		}

		public override void _ExitTree()
		{
			base._ExitTree();

			Unsubscribe();
		}

		private void Subscribe()
		{
			ClickHandler.OnClickedPc += PcManager.SelectPc;
			ClickHandler.OnClickedMoveTarget += PcManager.MoveTo;
			UI.MainMenu.OnNewGame += StartNewGame;
			UI.MainMenu.OnSaveGame += Save;
			UI.MainMenu.OnLoadGame += Load;
		}

		private void Unsubscribe()
		{
			ClickHandler.OnClickedPc -= PcManager.SelectPc;
			ClickHandler.OnClickedMoveTarget -= PcManager.MoveTo;
			UI.MainMenu.OnNewGame -= StartNewGame;
			UI.MainMenu.OnSaveGame -= Save;
			UI.MainMenu.OnLoadGame -= Load;
		}

		private void StartNewGame()
		{
			CreateNewLevel();
		}

		private void CreateNewLevel()
		{
			Level level = (Level)HomeBaseScene.Instantiate();
			CallDeferred(World.MethodName.AddChild, level);
			level.Initialize(TeamData);
			PcManager.Initialize(TeamData, InventoryManager);
			UI.Initialize(TeamData, InventoryManager);
		}

		private void Save()
		{
			SaverLoader.Save(InventoryManager, TeamData);
		}

		private void Load()
		{
			// Must load data before creating new level
			SaverLoader.Load(InventoryManager, TeamData);
			CreateNewLevel();
		}
	}
}
