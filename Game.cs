using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lastdew
{
    public partial class Game : Node
    {
	    private Camera Camera { get; set; }
        private ClickHandler ClickHandler { get; set; }
        private PcManager PcManager { get; set; }
        public UiManager Ui { get; private set; }
        private InventoryManager InventoryManager { get; set; }
        private List<BuildingData> Buildings { get; set; } = [];
        private TeamData TeamData { get; set; }
        private Level CurrentLevel { get; set; }
        public Fader Fader { get; private set; }
        private AudioStreamPlayer MusicPlayer { get; set; }
        private AudioStreamMP3 StartMenuSong { get; } = GD.Load<AudioStreamMP3>(Music.RELOAD_AND_REASSESS);
        
        #region TESTING STUFF

        [Export]
        private bool CombatTesting { get; set; }
        [Export]
        private int NumberOfPcs { get; set; } = 1;
        private List<PcSaveData> DefaultPcList { get; } = [];

        #endregion

        public override void _Ready()
		{
			base._Ready();

			Camera = GetNode<Camera>("%CameraRig");
			ClickHandler = Camera.ClickHandler;
			PcManager = GetNode<PcManager>("%PcManager");
			Ui = GetNode<UiManager>("%UiManager");
			TeamData = new TeamData();
			InventoryManager = new InventoryManager();
			Fader = GetNode<Fader>("%Fader");
			
			MusicPlayer = GetNode<AudioStreamPlayer>("%MusicPlayer");
			MusicPlayer.Stream = StartMenuSong;
			MusicPlayer.Play();
			
			// TODO: These should be run again in StartNewGame() and Load(), since 
			PcManager.Initialize(TeamData);
			Ui.ConnectSignals(TeamData, InventoryManager);

			GetTree().Paused = true;

			foreach (PcData data in Databases.PcDatas.PcDatas.Values)
			{
				DefaultPcList.Add(new PcSaveData(data.GetUid()));
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
		//	Ui.MainMenu.Exit.OnToStartMenu += ExitToStartMenu;
			Ui.MapMenu.OnStartScavenging += StartScavenging;
			Ui.MainMenu.OnReturnToBase += ReturnToBase;
			PcManager.OnLooted += Ui.Hud.AddToQueue;
			Ui.Hud.OnCenterOnPc += Camera.CenterOnPc;

			Ui.BuildMenu.Connect(
				BuildMenu.SignalName.OnBuild,
				Callable.From<Building3D>((building) => CurrentLevel.AddBuilding(building)));
			Camera.ClickHandler.Connect(
				ClickHandler.SignalName.OnPlacedBuilding,
				Callable.From<BuildingData>(PlaceBuilding));
			//Camera.ClickHandler.OnPlacedBuilding += PlaceBuilding;
		}

        private void UnsubscribeFromEvents()
		{
			ClickHandler.OnClickedPc -= PcManager.SelectPc;
			ClickHandler.OnClickedMoveTarget -= PcManager.MoveTo;
			Ui.MainMenu.OnNewGame -= StartNewGame;
			Ui.MainMenu.OnSaveGame -= Save;
			Ui.MainMenu.OnLoadGame -= Load;
			//Ui.MainMenu.Exit.OnToStartMenu -= ExitToStartMenu;
			Ui.MapMenu.OnStartScavenging -= StartScavenging;
			Ui.MainMenu.OnReturnToBase -= ReturnToBase;
			PcManager.OnLooted -= Ui.Hud.AddToQueue;
			Ui.Hud.OnCenterOnPc -= Camera.CenterOnPc;
			
			
			//Camera.ClickHandler.OnPlacedBuilding -= PlaceBuilding;
		}

		private async Task StartNewGame()
		{
			PackedScene homeBaseScene = GD.Load<PackedScene>(Uids.HOME_BASE);
			Ui.Initialize(TeamData, InventoryManager, Camera, Buildings);
			await SetupLevel(homeBaseScene, DefaultPcList, Buildings);
			Ui.ChangeState(new GameStateHome());
		}

		private async Task StartCombatTest()
		{
			PackedScene combatTestScene = GD.Load<PackedScene>("uid://dr032kqvigccx");
			Ui.Initialize(TeamData, InventoryManager, Camera, []);
			await SetupLevel(combatTestScene, DefaultPcList, []);
			Ui.ChangeState(new GameStateHome());
		}

		private void Save()
		{
			SaveSystem.Save(InventoryManager, TeamData, Buildings);
		}

		private async Task Load()
		{
			SaveData saveData = SaveSystem.Load();
			PackedScene homeBaseScene = GD.Load<PackedScene>(Uids.HOME_BASE);
			Buildings = SaveSystem.ConvertToBuildingDatas(saveData.BuildingDatas);
			Ui.Initialize(TeamData, InventoryManager, Camera, Buildings);
			// LoadInventory() needs to be called after SetupLevel() so the inventory UI (CharacterMenu)
			// can initialize first. It doesn't need to be called before, since it sends an empty InventoryManager,
			// and then LoadInventory just fills that object with items, so everyone who had it passed to them has
			// the correct inventory.
			await SetupLevel(homeBaseScene, saveData.PcSaveDatas, Buildings);
			LoadInventory(saveData.Inventory);
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

        private async Task SetupLevel(
	        PackedScene levelScene,
	        List<PcSaveData> pcSaveDatas,
	        List<BuildingData> buildingSaveDatas,
	        bool scavengingLevel = false)
		{
			Fader.FadeOut();
			await ToSignal(Fader, Fader.SignalName.OnFadeOut);
			
			CurrentLevel?.QueueFree();
			// TODO: Something going wrong here when starting, then quitting to menu, then starting again.
			// Freezes here.
			CurrentLevel = (Level)levelScene.Instantiate();
			CallDeferred(Node.MethodName.AddChild, CurrentLevel);
			CurrentLevel.Initialize(buildingSaveDatas, scavengingLevel);
			
			
			// UI.Setup() has to be called after PcManager.SpawnPcs(),
			// so TeamData will have the PlayerCharacter instance references (for HUD to use).
			PcManager.SpawnPcs(InventoryManager, pcSaveDatas);
			Ui.Setup();
			
			
			MusicPlayer.Stream = CurrentLevel.Song;
			MusicPlayer.Play();
			
			Fader.FadeIn();
		}
        
        private void ExitToStartMenu()
        {
	        PcManager.ClearPcs();
			CurrentLevel?.QueueFree();
			MusicPlayer.Stream = StartMenuSong;
			MusicPlayer.Play();
			Ui.ChangeState(new GameStateStart());
        }

		private async Task StartScavenging(PackedScene scene, List<PcSaveData> pcSaveDatas)
		{
			await SetupLevel(scene, pcSaveDatas, [], true);
			Ui.ChangeState(new GameStateScavenging());
		}
        
        private async Task ReturnToBase()
        {
			// Spawn returning pcs from sent Pcs, and the rest from saved PcSaveDatas.
			List<PcSaveData> pcSaveDatas = [];
			pcSaveDatas.AddRange(TeamData.Pcs.Select(pc => new PcSaveData(pc)));
			pcSaveDatas.AddRange(TeamData.UnusedPcDatas);
			TeamData.UnusedPcDatas.Clear();
			
			PackedScene homeBaseScene = GD.Load<PackedScene>(Uids.HOME_BASE);
			await SetupLevel(homeBaseScene, pcSaveDatas, Buildings);
			Ui.ChangeState(new GameStateHome());
        }

        private void PlaceBuilding(BuildingData data)
        {
	        Buildings.Add(data);
			CurrentLevel.NavMesh.BakeNavigationMesh();
	        
	        Building building = Databases.Craftables.Buildings[data.BuildingUid];
	        building.Purchase(InventoryManager);
        }
	}
}
