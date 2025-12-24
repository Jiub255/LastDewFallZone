using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lastdew
{
    public partial class Game : Node
    {
        public UiManager Ui { get; private set; }
        public Fader Fader { get; private set; }
        
	    private Camera Camera { get; set; }
        private PcManager PcManager { get; set; }
        private TeamData TeamData { get; set; }
        private Level CurrentLevel { get; set; }
        private AudioStreamPlayer MusicPlayer { get; set; }
        private AudioStreamMP3 StartMenuSong { get; } = GD.Load<AudioStreamMP3>(Music.RELOAD_AND_REASSESS);
        private TimeManager TimeManager { get; set; }
        
        
        #region TESTING STUFF

        [Export]
        private bool CombatTesting { get; set; }
        [Export]
        private int NumberOfPcs { get; set; } = 1;
        private List<PcSaveData> DefaultPcList { get; } = [];
        [Export]
        private float DayLengthInMinutes { get; set; } = 20f;

        #endregion

        public override void _Ready()
		{
			base._Ready();

			Camera = GetNode<Camera>("%CameraRig");
			PcManager = GetNode<PcManager>("%PcManager");
			Ui = GetNode<UiManager>("%UiManager");
			TeamData = new TeamData();
			Fader = GetNode<Fader>("%Fader");
			MusicPlayer = GetNode<AudioStreamPlayer>("%MusicPlayer");
			TimeManager = new TimeManager(DayLengthInMinutes);
			
			MusicPlayer.Stream = StartMenuSong;
			MusicPlayer.Play();
			
			PcManager.Initialize(TeamData);
			Ui.ConnectSignals(TeamData);

			GetTree().Paused = true;

			foreach (PcData data in Databases.PcDatas.PcDatas.Values)
			{
				DefaultPcList.Add(new PcSaveData(data));
			}

			SubscribeToEvents();

			if (CombatTesting)
			{
                StartCombatTest();
            }
		}

		public override void _Process(double delta)
		{
			base._Process(delta);
			
			TimeManager?.Process((float)delta);
		}

		public override void _ExitTree()
		{
			base._ExitTree();

			UnsubscribeFromEvents();
		}

		private void SubscribeToEvents()
		{
			Camera.ClickHandlerGame.OnClickedPc += PcManager.SelectPc;
			Camera.ClickHandlerGame.OnClickedMoveTarget += PcManager.MoveTo;
			Ui.MainMenu.OnNewGame += StartNewGame;
			Ui.MainMenu.OnSaveGame += Save;
			Ui.MainMenu.OnLoadGame += Load;
			Ui.MapMenu.OnStartScavenging += StartScavenging;
			Ui.MainMenu.OnReturnToBase += ReturnToBase;
			PcManager.OnLooted += Ui.Hud.AddToQueue;
			Ui.Hud.OnCenterOnPc += Camera.CenterOnPc;
			
			Camera.ClickHandlerBuild.Connect(
				ClickHandlerBuild.SignalName.OnPlacedBuilding,
				Callable.From<BuildingData>(PlaceBuilding));
			//Camera.ClickHandler.OnPlacedBuilding += PlaceBuilding;
		}

        private void UnsubscribeFromEvents()
		{
			Camera.ClickHandlerGame.OnClickedPc -= PcManager.SelectPc;
			Camera.ClickHandlerGame.OnClickedMoveTarget -= PcManager.MoveTo;
			Ui.MainMenu.OnNewGame -= StartNewGame;
			Ui.MainMenu.OnSaveGame -= Save;
			Ui.MainMenu.OnLoadGame -= Load;
			Ui.MapMenu.OnStartScavenging -= StartScavenging;
			Ui.MainMenu.OnReturnToBase -= ReturnToBase;
			PcManager.OnLooted -= Ui.Hud.AddToQueue;
			Ui.Hud.OnCenterOnPc -= Camera.CenterOnPc;
			
			
			//Camera.ClickHandler.OnPlacedBuilding -= PlaceBuilding;
		}

		private async Task StartNewGame()
		{
			PackedScene homeBaseScene = GD.Load<PackedScene>(Uids.HOME_BASE);
			Ui.Initialize(TeamData, Camera, TimeManager);
			await SetupLevel(homeBaseScene, DefaultPcList, TeamData.Inventory.Buildings);
			Ui.ChangeState(new GameStateHome());
		}

		private async Task StartCombatTest()
		{
			PackedScene combatTestScene = GD.Load<PackedScene>("uid://dr032kqvigccx");
			Ui.Initialize(TeamData, Camera, TimeManager);
			await SetupLevel(combatTestScene, DefaultPcList, []);
			Ui.ChangeState(new GameStateHome());
		}

		private void Save()
		{
			SaveSystem.Save(TeamData);
		}

		private async Task Load()
		{
			SaveData saveData = SaveSystem.Load();
			PackedScene homeBaseScene = GD.Load<PackedScene>(Uids.HOME_BASE);
			TeamData.Inventory.Buildings = SaveSystem.ConvertToBuildingDatas(saveData.BuildingDatas);
			Ui.Initialize(TeamData, Camera, TimeManager);
			// LoadInventory() needs to be called after SetupLevel() so the inventory UI (CharacterMenu)
			// can initialize first.
			await SetupLevel(homeBaseScene, saveData.PcSaveDatas, TeamData.Inventory.Buildings);
			FillInventory(saveData.Inventory);
			Ui.ChangeState(new GameStateHome());
		}

		private void FillInventory(Dictionary<long, int> inventory)
		{
			Craftables craftables = Databases.Craftables;
			foreach (KeyValuePair<long, int> kvp in inventory)
			{
				TeamData.Inventory.AddItems((Item)craftables[kvp.Key], kvp.Value);
			}
		}

        private async Task SetupLevel(
	        PackedScene levelScene,
	        List<PcSaveData> pcSaveDatas,
	        List<BuildingData> buildingSaveDatas)
		{
			Fader.FadeOut();
			await ToSignal(Fader, Fader.SignalName.OnFadeOut);
			
			CurrentLevel?.QueueFree();
			CurrentLevel = (Level)levelScene.Instantiate();
			this.AddChildDeferred(CurrentLevel);

			if (CurrentLevel is HomeBase homeBase)
			{
				homeBase.Initialize(buildingSaveDatas);
				Ui.BuildMenu.Connect(BuildMenu.SignalName.OnBuild,
					Callable.From<Building3D>(building3D => homeBase.AddBuilding(building3D)));
				TimeManager.Initialize(homeBase);
			}
			else
			{
				CurrentLevel.Initialize(buildingSaveDatas);
				// TODO: Necessary to set TimeManager.HomeBase to null here?
				// Or does the queue free take care of that?
			}
			
			// UI.Setup() has to be called after PcManager.SpawnPcs(),
			// so TeamData will have the PlayerCharacter instance references (for HUD to use).
			PcManager.SpawnPcs(TeamData.Inventory, pcSaveDatas);
			Ui.Setup();
			
			MusicPlayer.Stream = CurrentLevel.Song;
			MusicPlayer.Play();
			
			Fader.FadeIn();
		}

		private async Task StartScavenging(PackedScene scene, List<PcSaveData> pcSaveDatas)
		{
			await SetupLevel(scene, pcSaveDatas, []);
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
			await SetupLevel(homeBaseScene, pcSaveDatas, TeamData.Inventory.Buildings);
			Ui.ChangeState(new GameStateHome());
        }

        private void PlaceBuilding(BuildingData data)
        {
	        TeamData.Inventory.Buildings.Add(data);

	        if (CurrentLevel is HomeBase homeBase)
	        {
				homeBase.NavMesh.BakeNavigationMesh();
	        }
	        else
	        {
		        GD.PushError("Can't place building outside of home base.");
	        }
	        
	        Building building = Databases.Craftables.Buildings[data.BuildingUid];
	        building.Purchase(TeamData.Inventory);
        }
	}
}
