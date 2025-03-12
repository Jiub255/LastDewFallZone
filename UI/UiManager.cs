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
		private bool AnyMenuOpen { get; set; }
		private GameState CurrentState { get; set; }
		
		public override void _Ready()
		{
			MainMenu = GetNode<MainMenu>("%MainMenu");
			Hud = GetNode<Hud>("%HUD");
			GameMenu = GetNode<GameMenu>("%GameMenu");
			BuildMenu = GetNode<BuildMenu>("%BuildMenu");
			MapMenu = GetNode<MapMenu>("%MapMenu");

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

        public override void _Input(InputEvent @event)
		{
			base._Input(@event);
			
			if (AnyMenuOpen && @event.IsActionPressed(InputNames.EXIT_MENU))
			{
				CloseAllMenus();
				return;
			}

			CurrentState.HandleInput(@event);
		}
		
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			Hud.Initialize(teamData);
			GameMenu.Initialize(teamData, inventoryManager);
			MapMenu.Initialize(teamData);

			MainMenu.Close();
			Hud.Open();
			
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
			this.PrintDebug($"Entering {gameState.GetType()}");
        }
		
		private void Toggle(Menu menu)
		{
			if (menu.Visible)
			{
				CloseAllMenus();
			}
			else
			{
				if (AnyMenuOpen)
				{
					CloseAllMenus();
				}
				menu.Open();
				Hud.Close();
				AnyMenuOpen = true;
				GetTree().Paused = true;
			}
		}

		private void CloseAllMenus()
		{
			GameMenu.Close();
			BuildMenu.Close();
			MapMenu.Close();
			MainMenu.Close();
			
			Hud.Open();
			
			AnyMenuOpen = false;
			GetTree().Paused = false;
		}
        
        private void SubscribeState(GameState state)
        {
			state.OnToggleMain += () => Toggle(MainMenu);
			state.OnToggleGame += () => Toggle(GameMenu);
			state.OnToggleBuild += () => Toggle(BuildMenu);
			state.OnToggleMap += () => Toggle(MapMenu);
        }
        
        private void UnsubscribeState(GameState state)
        {
			state.OnToggleMain -= () => Toggle(MainMenu);
			state.OnToggleGame -= () => Toggle(GameMenu);
			state.OnToggleBuild -= () => Toggle(BuildMenu);
			state.OnToggleMap -= () => Toggle(MapMenu);
        }
	}
}
