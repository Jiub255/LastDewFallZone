using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class World : Node3D
	{
		private ClickHandler ClickHandler { get; set; }
		private PcManager PcManager { get; set; }
		private UiManager UI { get; set; }
		private InventoryManager InventoryManager { get; set; }
		private TeamData TeamData { get; set; }
		private PackedScene HomeBaseScene { get; } = GD.Load<PackedScene>("res://world/home-test-env.tscn");
		
		public override void _Ready()
		{
			base._Ready();

			Camera camera = GetNode<Camera>("%CameraRig");
			ClickHandler = camera.ClickHandler;
			PcManager = GetNode<PcManager>("%PcManager");
			UI = GetNode<UiManager>("%UiManager");
			TeamData = new TeamData();
			InventoryManager = new InventoryManager();
			
			PcManager.Initialize(TeamData);

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
			UI.MapMenu.OnStartScavenging += StartScavenging;
		}

		private void Unsubscribe()
		{
			ClickHandler.OnClickedPc -= PcManager.SelectPc;
			ClickHandler.OnClickedMoveTarget -= PcManager.MoveTo;
			UI.MainMenu.OnNewGame -= StartNewGame;
			UI.MainMenu.OnSaveGame -= Save;
			UI.MainMenu.OnLoadGame -= Load;
			UI.MapMenu.OnStartScavenging -= StartScavenging;
		}

		// JUST FOR TESTING
		private List<PcSaveData> DefaultPcList = new List<PcSaveData>()
		{
			new PcSaveData()
		};

		private void StartNewGame()
		{
			SetupLevel(HomeBaseScene, DefaultPcList);
		}

		private void Save()
		{
			SaverLoader.Save(InventoryManager, TeamData);
		}

		private void Load()
		{
			SaveData saveData = SaverLoader.Load();
			LoadInventory(saveData);
			SetupLevel(HomeBaseScene, saveData.PcSaveDatas);
		}

		private void StartScavenging(PackedScene scene, List<PcSaveData> pcSaveDatas)
		{
			SetupLevel(scene, pcSaveDatas);
		}

		private void SetupLevel(PackedScene scene, List<PcSaveData> pcSaveDatas)
		{
			foreach (Node node in GetChildren())
			{
				if (node is Level oldLevel)
				{
					oldLevel.QueueFree();
				}
			}
			Level level = (Level)scene.Instantiate();
			CallDeferred(World.MethodName.AddChild, level);
			level.Initialize(TeamData);
			PcManager.SpawnPcs(InventoryManager, pcSaveDatas);
			// UI.Initialize has to be called after PcManager.SpawnPcs,
			// so TeamData will have the PlayerCharacter instance references (for HUD to use).
			UI.Initialize(TeamData, InventoryManager);
		}

		private void LoadInventory(SaveData saveData)
		{
			Craftables craftables = ResourceLoader.Load<Craftables>("res://craftables/craftables.tres");
			foreach (KeyValuePair<string, int> kvp in saveData.Inventory)
			{
				InventoryManager.AddItems((Item)craftables[kvp.Key], kvp.Value);
			}
		}
	}
}
