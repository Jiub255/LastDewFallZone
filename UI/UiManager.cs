using Godot;

namespace Lastdew
{
	public partial class UiManager : Control
	{
		public MainMenu MainMenu { get; private set; }
		public Hud Hud { get; private set; }
		public BuildMenu BuildMenu { get; private set; }
		public MapMenu MapMenu { get; private set; }
		public MissionSummaryMenu MissionSummaryMenu { get; private set; }
		private CharacterMenu CharacterMenu { get; set; }
		private CraftingMenu CraftingMenu { get; set; }
		private OptionsMenu OptionsMenu { get; set; }
		
		private GameState CurrentState { get; set; }
		private bool AnyMenuOpen { get; set; }
		
		public override void _Ready()
		{
			MainMenu = GetNode<MainMenu>("%MainMenu");
			Hud = GetNode<Hud>("%HUD");
			CharacterMenu = GetNode<CharacterMenu>("%CharacterMenu");
			CraftingMenu = GetNode<CraftingMenu>("%CraftingMenu");
			BuildMenu = GetNode<BuildMenu>("%BuildMenu");
			MapMenu = GetNode<MapMenu>("%MapMenu");
			OptionsMenu = GetNode<OptionsMenu>("%OptionsMenu");
			MissionSummaryMenu = GetNode<MissionSummaryMenu>("%MissionSummaryMenu");

			ConnectHudButtons();

			CloseMenu(Hud);
			CloseMenu(CharacterMenu);
			CloseMenu(CraftingMenu);
			CloseMenu(BuildMenu);
			CloseMenu(MapMenu);
			CloseMenu(OptionsMenu);
			CloseMenu(MissionSummaryMenu);

			ChangeState(new GameStateStart());			
		}

        public override void _ExitTree()
        {
            base._ExitTree();
            
			DisconnectHudButtons();
            if (CurrentState != null)
            {
                UnsubscribeState(CurrentState);
            }
            UnsubscribeFromEvents();
        }

        public override void _Process(double delta)
        {
	        base._Process(delta);

	        if (AnyMenuOpen && Input.IsActionJustReleased(InputNames.EXIT_MENU))
	        {
		        CloseAllMenus();
		        return;
	        }
	        
	        CurrentState.ProcessState();
        }

        public void SubscribeToEvents(TeamData teamData)
        {
	        BuildMenu.SubscribeToEvents(teamData.Inventory);
	        CraftingMenu.SubscribeToEvents(teamData.Inventory);
	        CharacterMenu.SubscribeToEvents(teamData);

	        MainMenu.Options.Pressed += ToggleOptionsMenu;
	        MainMenu.ReturnToGame.Pressed += ToggleMainMenu;
	        OptionsMenu.Back.Pressed += ToggleMainMenu;
        }
		
        /// <summary>
        /// Only called once in beginning (either starting the game fresh or after pressing quit to main menu)
        /// </summary>
		public void Initialize(TeamData teamData,
	        Camera camera,
	        TimeManager timeManager,
	        ExperienceFormula formula)
		{
			Hud.Initialize(teamData, timeManager);
			CharacterMenu.Initialize(teamData);
			CraftingMenu.Initialize(teamData);
			MapMenu.Initialize(teamData);
			BuildMenu.Initialize(teamData, camera);
			MissionSummaryMenu.Initialize(formula);
		}

        /// <summary>
        /// Called each time you switch levels from Game.SetupLevel().
        /// </summary>
		public void Setup()
		{
			Hud.Setup();
			CharacterMenu.Setup();
			CraftingMenu.Setup();
			BuildMenu.Setup();
			
			CloseMenu(MainMenu);
			OpenMenu(Hud);
			AnyMenuOpen = false;
			
			GetTree().Paused = false;
		}
        
        public void ChangeState(GameState gameState)
        {
			if (CurrentState != null)
			{
				UnsubscribeState(CurrentState);
				GD.Print($"Changing game state from {CurrentState.GetType().Name} to {gameState.GetType().Name}");
			}
			CurrentState = gameState;
			SubscribeState(gameState);
			CurrentState.EnterState(this);
        }
		
		private void Toggle(Menu menu)
		{
			if (menu.Visible)
			{
				CloseAllMenus();
			}
			else
			{
				CloseAllMenus(false);
				OpenMenu(menu);
			}
		}

		private void ToggleMainMenu()
		{
			Toggle(MainMenu);
		}

		private void ToggleCraftingMenu()
		{
			Toggle(CraftingMenu);
		}

		private void ToggleCharacterMenu()
		{
			Toggle(CharacterMenu);
		}

		private void ToggleMapMenu()
		{
			Toggle(MapMenu);
		}

		private void ToggleBuildMenu()
		{
			Toggle(BuildMenu);
		}

		private void ToggleOptionsMenu()
		{
			Toggle(OptionsMenu);
		}

		private void OpenMenu(Menu menu)
		{
			if (menu.Visible)
			{
				return;
			}
			
			menu.Open();
			
			if (menu == Hud)
			{
				return;
			}
			
			AnyMenuOpen = true;
			GetTree().Paused = true;
		}

		private static void CloseMenu(Menu menu)
		{
			if (!menu.Visible)
			{
				return;
			}

			menu.Close();
		}

		private void CloseAllMenus(bool exceptHud = true)
		{
			CloseMenu(BuildMenu);
			CloseMenu(CharacterMenu);
			CloseMenu(CraftingMenu);
			CloseMenu(MainMenu);
			CloseMenu(MapMenu);
			CloseMenu(OptionsMenu);

			if (exceptHud)
			{
				OpenMenu(Hud);
			}
			else
			{
				CloseMenu(Hud);
			}
			
			AnyMenuOpen = false;
			GetTree().Paused = false;
		}
        
        private void SubscribeState(GameState state)
        {
			state.OnToggleMain += ToggleMainMenu;
			state.OnToggleCharacter += ToggleCharacterMenu;
			state.OnToggleCrafting += ToggleCraftingMenu;
			state.OnToggleBuild += ToggleBuildMenu;
			state.OnToggleMap += ToggleMapMenu;
        }
        
        private void UnsubscribeState(GameState state)
        {
	        state.OnToggleMain -= ToggleMainMenu;
	        state.OnToggleCharacter -= ToggleCharacterMenu;
	        state.OnToggleCrafting -= ToggleCraftingMenu;
	        state.OnToggleBuild -= ToggleBuildMenu;
	        state.OnToggleMap -= ToggleMapMenu;
        }

        private void ConnectHudButtons()
        {
	        Hud.Build.Pressed += ToggleBuildMenu;
	        Hud.Craft.Pressed += ToggleCraftingMenu;
	        Hud.Character.Pressed += ToggleCharacterMenu;
	        Hud.Map.Pressed += ToggleMapMenu;
	        Hud.Main.Pressed += ToggleMainMenu;
        }
        
        private void DisconnectHudButtons()
        {
	        Hud.Build.Pressed -= ToggleBuildMenu;
	        Hud.Craft.Pressed -= ToggleCraftingMenu;
	        Hud.Character.Pressed -= ToggleCharacterMenu;
	        Hud.Map.Pressed -= ToggleMapMenu;
	        Hud.Main.Pressed -= ToggleMainMenu;
        }

        private void UnsubscribeFromEvents()
        {
	        MainMenu.Options.Pressed -= ToggleOptionsMenu;
	        MainMenu.ReturnToGame.Pressed -= ToggleMainMenu;
	        OptionsMenu.Back.Pressed -= ToggleMainMenu;
        }
	}
}
