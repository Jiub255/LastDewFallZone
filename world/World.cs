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
        private PackedScene HomeBaseScene { get; } = GD.Load<PackedScene>(UIDs.HOME_BASE);
        private Level HomeBase { get; set; }
        private ScavengingLevel ScavengingLevel { get; set; }

        // TESTING STUFF

        private PackedScene CombatTestScene { get; } = GD.Load<PackedScene>("uid://dr032kqvigccx");
        [Export]
        private bool CombatTesting { get; set; } = false;

        // END OF TESTING STUFF


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

			SubscribeToEvents();

			if (CombatTesting)
			{
                StartCombatTest();
            }
		}

		public override void _ExitTree()
		{
			base._ExitTree();

			UnsubscribeFromEvents();
		}

		private void SubscribeToEvents()
		{
			ClickHandler.OnClickedPc += PcManager.SelectPc;
			ClickHandler.OnClickedMoveTarget += PcManager.MoveTo;
			UI.MainMenu.OnNewGame += StartNewGame;
			UI.MainMenu.OnSaveGame += Save;
			UI.MainMenu.OnLoadGame += Load;
			UI.MainMenu.Exit.OnToStartMenu += ExitToStartMenu;
			UI.MapMenu.OnStartScavenging += StartScavenging;
			UI.MainMenu.ReturnToBase.Pressed += ReturnToBase;
		}

        private void UnsubscribeFromEvents()
		{
			ClickHandler.OnClickedPc -= PcManager.SelectPc;
			ClickHandler.OnClickedMoveTarget -= PcManager.MoveTo;
			UI.MainMenu.OnNewGame -= StartNewGame;
			UI.MainMenu.OnSaveGame -= Save;
			UI.MainMenu.OnLoadGame -= Load;
			UI.MainMenu.Exit.OnToStartMenu -= ExitToStartMenu;
			UI.MapMenu.OnStartScavenging -= StartScavenging;
			UI.MainMenu.ReturnToBase.Pressed -= ReturnToBase;
		}

		// JUST FOR TESTING
		private readonly List<PcSaveData> DefaultPcList =
        [
            new PcSaveData()
		];

		private void StartNewGame()
		{
			HomeBase = SetupLevel(HomeBaseScene, DefaultPcList);
			UI.ChangeState(new GameStateHome());
		}

		private void StartCombatTest()
		{
			HomeBase = SetupLevel(CombatTestScene, DefaultPcList);
			UI.ChangeState(new GameStateHome());
		}

		private void Save()
		{
			SaverLoader.Save(InventoryManager, TeamData);
		}

		private void Load()
		{
			SaveData saveData = SaverLoader.Load();
			LoadInventory(saveData);
			HomeBase = SetupLevel(HomeBaseScene, saveData.PcSaveDatas);
			UI.ChangeState(new GameStateHome());
		}

		private void LoadInventory(SaveData saveData)
		{
			Craftables craftables = Databases.CRAFTABLES;
			foreach (KeyValuePair<long, int> kvp in saveData.Inventory)
			{
				InventoryManager.AddItems((Item)craftables[kvp.Key], kvp.Value);
			}
		}

        private Level SetupLevel(PackedScene levelScene, List<PcSaveData> pcSaveDatas)
		{
			Level level = (Level)levelScene.Instantiate();
			CallDeferred(World.MethodName.AddChild, level);
			level.Initialize(TeamData);
			// UI.Initialize has to be called after PcManager.SpawnPcs,
			// so TeamData will have the PlayerCharacter instance references (for HUD to use).
			PcManager.SpawnPcs(InventoryManager, pcSaveDatas);
			UI.Initialize(TeamData, InventoryManager);
			return level;
		}
        
        private void ExitToStartMenu()
        {
			foreach (Node node in GetChildren())
			{
				if (node is Level oldLevel)
				{
					oldLevel.QueueFree();
				}
			}
			UI.ChangeState(new GameStateStart());
        }

		private void StartScavenging(PackedScene scene, List<PcSaveData> pcSaveDatas)
		{
			RemoveChild(HomeBase);
			ScavengingLevel = (ScavengingLevel)SetupLevel(scene, pcSaveDatas);
			UI.ChangeState(new GameStateScavenging());
		}
        
        private void ReturnToBase()
        {
			ScavengingLevel?.QueueFree();
			AddChild(HomeBase);
			
			// Spawn returning pcs from sent PcDatas, and the rest from saved PcDatas.
			List<PcSaveData> pcSaveDatas = new();
			foreach (PlayerCharacter pc in TeamData.Pcs)
			{
				PcSaveData pcSaveData = new(pc);
				pcSaveDatas.Add(pcSaveData);
			}
			pcSaveDatas.AddRange(TeamData.UnusedPcDatas);
			
			PcManager.SpawnPcs(InventoryManager, pcSaveDatas);
			UI.Initialize(TeamData, InventoryManager);
			UI.ChangeState(new GameStateHome());
        }
	}
}
