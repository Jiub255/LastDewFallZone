using Godot;

namespace Lastdew
{
	public partial class UiManager : Control
	{
		public MainMenu MainMenu { get; private set; }
		public Hud Hud { get; private set; }
		public GameMenu GameMenu { get; private set; }
		public BuildMenu BuildMenu { get; private set; }
		public MapMenu MapMenu { get; private set; }
		
		private GameState CurrentState { get; set; }
		private bool AnyMenuOpen { get; set; }
		
		public override void _Ready()
		{
			MainMenu = GetNode<MainMenu>("%MainMenu");
			Hud = GetNode<Hud>("%HUD");
			GameMenu = GetNode<GameMenu>("%GameMenu");
			BuildMenu = GetNode<BuildMenu>("%BuildMenu");
			MapMenu = GetNode<MapMenu>("%MapMenu");

			CloseMenu(Hud);
			CloseMenu(GameMenu);
			CloseMenu(BuildMenu);
			CloseMenu(MapMenu);

			ChangeState(new GameStateStart());			
		}

        public override void _ExitTree()
        {
            base._ExitTree();
            
            if (CurrentState != null)
            {
                UnsubscribeState(CurrentState);
            }
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
		
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			Hud.Initialize(teamData);
			GameMenu.Initialize(teamData, inventoryManager);
			MapMenu.Initialize(teamData);

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
			}
			CurrentState = gameState;
			SubscribeState(gameState);
			CurrentState.EnterState(MainMenu);
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

		private void ToggleGameMenu()
		{
			Toggle(GameMenu);
		}

		private void ToggleMapMenu()
		{
			Toggle(MapMenu);
		}

		private void ToggleBuildMenu()
		{
			Toggle(BuildMenu);
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
			CloseMenu(GameMenu);
			CloseMenu(BuildMenu);
			CloseMenu(MapMenu);
			CloseMenu(MainMenu);

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
			state.OnToggleGame += ToggleGameMenu;
			state.OnToggleBuild += ToggleBuildMenu;
			state.OnToggleMap += ToggleMapMenu;
        }
        
        private void UnsubscribeState(GameState state)
        {
	        state.OnToggleMain -= ToggleMainMenu;
	        state.OnToggleGame -= ToggleGameMenu;
	        state.OnToggleBuild -= ToggleBuildMenu;
	        state.OnToggleMap -= ToggleMapMenu;
        }
	}
}
