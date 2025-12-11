using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lastdew
{
    public partial class Game : Node
    {
        private ClickHandler ClickHandler { get; set; }
        private PcManager PcManager { get; set; }
        private UiManager Ui { get; set; }
        private InventoryManager InventoryManager { get; set; }
        private TeamData TeamData { get; set; }
        private Level CurrentLevel { get; set; }
        private Fader Fader { get; set; }
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

			Camera camera = GetNode<Camera>("%CameraRig");
			ClickHandler = camera.ClickHandler;
			PcManager = GetNode<PcManager>("%PcManager");
			Ui = GetNode<UiManager>("%UiManager");
			TeamData = new TeamData();
			InventoryManager = new InventoryManager();
			Fader = GetNode<Fader>("%Fader");
			
			MusicPlayer = GetNode<AudioStreamPlayer>("%MusicPlayer");
			MusicPlayer.Stream = StartMenuSong;
			MusicPlayer.Play();
			
			PcManager.Initialize(TeamData);

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
			Ui.MainMenu.Exit.OnToStartMenu += ExitToStartMenu;
			Ui.MapMenu.OnStartScavenging += StartScavenging;
			Ui.MainMenu.OnReturnToBase += ReturnToBase;
			PcManager.OnLooted += Ui.Hud.ShowLootedItems;
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
			Ui.MainMenu.OnReturnToBase -= ReturnToBase;
			PcManager.OnLooted -= Ui.Hud.ShowLootedItems;
		}

		private async Task StartNewGame()
		{
			PackedScene homeBaseScene = GD.Load<PackedScene>(Uids.HOME_BASE);
			await SetupLevel(homeBaseScene, DefaultPcList);
			Ui.ChangeState(new GameStateHome());
		}

		private async Task StartCombatTest()
		{
			PackedScene combatTestScene = GD.Load<PackedScene>("uid://dr032kqvigccx");
			await SetupLevel(combatTestScene, DefaultPcList);
			Ui.ChangeState(new GameStateHome());
		}

		private void Save()
		{
			SaveSystem.Save(InventoryManager, TeamData);
		}

		private async Task Load()
		{
			SaveData saveData = SaveSystem.Load();
			PackedScene homeBaseScene = GD.Load<PackedScene>(Uids.HOME_BASE);
			await SetupLevel(homeBaseScene, saveData.PcSaveDatas);
			// LoadInventory needs to be called after SetupLevel so the inventory UI (CharacterMenu) can initialize first.
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

        private async Task SetupLevel(PackedScene levelScene, List<PcSaveData> pcSaveDatas)
		{
			Fader.FadeOut();
			await ToSignal(Fader, Fader.SignalName.OnFadeOut);
			
			CurrentLevel?.QueueFree();
			
			CurrentLevel = (Level)levelScene.Instantiate();
			CallDeferred(Node.MethodName.AddChild, CurrentLevel);
			CurrentLevel.Initialize(TeamData);
			
			// UI.Initialize has to be called after PcManager.SpawnPcs,
			// so TeamData will have the PlayerCharacter instance references (for HUD to use).
			PcManager.SpawnPcs(InventoryManager, pcSaveDatas);
			Ui.Initialize(TeamData, InventoryManager);
			
			MusicPlayer.Stream = CurrentLevel.Song;
			MusicPlayer.Play();
			
			Fader.FadeIn();
		}
        
        private void ExitToStartMenu()
        {
			CurrentLevel?.QueueFree();
			MusicPlayer.Stream = StartMenuSong;
			MusicPlayer.Play();
			Ui.ChangeState(new GameStateStart());
        }

		private async Task StartScavenging(PackedScene scene, List<PcSaveData> pcSaveDatas)
		{
			await SetupLevel(scene, pcSaveDatas);
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
			await SetupLevel(homeBaseScene, pcSaveDatas);
			Ui.ChangeState(new GameStateHome());
        }
	}
}
