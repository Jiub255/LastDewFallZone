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
		private PackedScene HomeBaseScene { get; } = GD.Load<PackedScene>("res://game/world/home-test-env.tscn");
		
		public override void _Ready()
		{
			base._Ready();

			Camera camera = GetNode<Camera>("%CameraRig");
			ClickHandler = camera.ClickHandler;
			PcManager = GetNode<PcManagerBase>("%PcManager");
			UI = GetNode<UiManager>("%UiManager");
			TeamData = new TeamData();
			InventoryManager = new InventoryManager();

			GetTree().Paused = true;

			Subscribe();
			
			//this.PrintDebug($"Global user data directory: {OS.GetDataDir()}");
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

		// JUST FOR TESTING
		private List<PcSaveData> DefaultPcList = new List<PcSaveData>()
		{
			new PcSaveData()
		};

		private void StartNewGame()
		{
			CreateNewLevel(DefaultPcList);
		}

		private void CreateNewLevel(List<PcSaveData> pcSaveDatas)
		{
			Level level = (Level)HomeBaseScene.Instantiate();
			CallDeferred(World.MethodName.AddChild, level);
			level.Initialize(TeamData);
			PcManager.Initialize(TeamData, InventoryManager, pcSaveDatas);
			UI.Initialize(TeamData, InventoryManager);
		}

		private void Save()
		{
			SaverLoader.Save(InventoryManager, TeamData);
		}

		private void Load()
		{
			// Must load data before creating new level
			List<PcSaveData> pcSaveDatas = SaverLoader.Load(InventoryManager);
			CreateNewLevel(pcSaveDatas);
		}
	}
}
