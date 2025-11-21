using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Lastdew
{
    public partial class Game : Node
    {
        private ClickHandler ClickHandler { get; set; }
        private PcManager PcManager { get; set; }
        private UiManager Ui { get; set; }
        private InventoryManager InventoryManager { get; set; }
        private TeamData TeamData { get; set; }
        private PackedScene HomeBaseScene { get; } = GD.Load<PackedScene>(Uids.HOME_BASE);
        private Level HomeBase { get; set; }
        private ScavengingLevel ScavengingLevel { get; set; }

#region TESTING STUFF

        [Export]
        private bool CombatTesting { get; set; }
        [Export]
        private int NumberOfPcs { get; set; } = 1;
        private PackedScene CombatTestScene { get; } = GD.Load<PackedScene>("uid://dr032kqvigccx");
        private List<PcSaveData> DefaultPcList { get; } = [];
		private List<PcData> DefaultPcDatas { get; }= [
			GD.Load<PcData>("uid://bqd6uonxmwcas"),
			GD.Load<PcData>("uid://cvscwbigsi3w3"),
			GD.Load<PcData>("uid://cyk852026vmce"),
			GD.Load<PcData>("uid://hiyhdi8pggjm"),
		];

#endregion

        public override void _Ready()
		{
			base._Ready();

			Camera camera = GetNode<Camera>("%CameraRig");
			ClickHandler = camera.ClickHandler;
			PcManager = GetNode<PcManager>("%PcManager");
			Ui = GetNode<UiManager>("%UiManager");
			TeamData = new TeamData();
			InventoryManager = new InventoryManager();
			
			PcManager.Initialize(TeamData);

			GetTree().Paused = true;

			for (int i = 0; i < NumberOfPcs; i++)
			{
                DefaultPcList.Add(new PcSaveData(DefaultPcDatas[i % DefaultPcDatas.Count]));
            }

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
			Ui.MainMenu.OnNewGame += StartNewGame;
			Ui.MainMenu.OnSaveGame += Save;
			Ui.MainMenu.OnLoadGame += Load;
			Ui.MainMenu.Exit.OnToStartMenu += ExitToStartMenu;
			Ui.MapMenu.OnStartScavenging += StartScavenging;
			Ui.MainMenu.ReturnToBase.Pressed += ReturnToBase;
		}

        private void UnsubscribeFromEvents()
		{
			ClickHandler.OnClickedPc -= PcManager.SelectPc;
			ClickHandler.OnClickedMoveTarget -= PcManager.MoveTo;
			Ui.MainMenu.OnNewGame -= StartNewGame;
			Ui.MainMenu.OnSaveGame -= Save;
			Ui.MainMenu.OnLoadGame -= Load;
			Ui.MainMenu.Exit.OnToStartMenu -= ExitToStartMenu;
			Ui.MapMenu.OnStartScavenging -= StartScavenging;
			Ui.MainMenu.ReturnToBase.Pressed -= ReturnToBase;
		}

		private /*async */void StartNewGame()
		{
			/*await */SetupLevel(HomeBaseScene, DefaultPcList);
			Ui.ChangeState(new GameStateHome());
		}

		private void StartCombatTest()
		{
			SetupLevel(CombatTestScene, DefaultPcList);
			Ui.ChangeState(new GameStateHome());
		}

		private void Save()
		{
			SaveSystem.Save(InventoryManager, TeamData);
		}

		private void Load()
		{
			SaveData saveData = SaveSystem.Load();
			LoadInventory(saveData.Inventory);
			SetupLevel(HomeBaseScene, saveData.PcSaveDatas);
			Ui.ChangeState(new GameStateHome());
		}

		private void LoadInventory(Dictionary<long, int> inventory)
		{
			Craftables craftables = Databases.Craftables;
			foreach (KeyValuePair<long, int> kvp in inventory)
			{
				InventoryManager.AddItems((Item)craftables[kvp.Key], kvp.Value);
			}
		}

        private /*async System.Threading.Tasks.Task*/ void SetupLevel(PackedScene levelScene, List<PcSaveData> pcSaveDatas, bool scavenging = false)
		{
			Level level = (Level)levelScene.Instantiate();
			CallDeferred(Node.MethodName.AddChild, level);
			level.Initialize(TeamData);
			// UI.Initialize has to be called after PcManager.SpawnPcs,
			// so TeamData will have the PlayerCharacter instance references (for HUD to use).
			Ui.MainMenu.Close();
			/*await */PcManager.SpawnPcs(InventoryManager, pcSaveDatas);
			Ui.Initialize(TeamData, InventoryManager);
			if (scavenging)
			{
				ScavengingLevel = (ScavengingLevel)level;
			}
			else
			{
				HomeBase = level;
			}
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
			Ui.ChangeState(new GameStateStart());
        }

		private void StartScavenging(PackedScene scene, List<PcSaveData> pcSaveDatas)
		{
			RemoveChild(HomeBase);
			SetupLevel(scene, pcSaveDatas, true);
			Ui.ChangeState(new GameStateScavenging());
		}
        
        private void ReturnToBase()
        {
			ScavengingLevel?.QueueFree();
			AddChild(HomeBase);
			
			// Spawn returning pcs from sent PcDatas, and the rest from saved PcDatas.
			List<PcSaveData> pcSaveDatas = [];
			pcSaveDatas.AddRange(TeamData.Pcs.Select(pc => new PcSaveData(pc)));
			pcSaveDatas.AddRange(TeamData.UnusedPcDatas);
			
			PcManager.SpawnPcs(InventoryManager, pcSaveDatas);
			Ui.Initialize(TeamData, InventoryManager);
			Ui.ChangeState(new GameStateHome());
        }
	}
}
