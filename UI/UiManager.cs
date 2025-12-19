using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class UiManager : Control
	{
		public MainMenu MainMenu { get; private set; }
		public Hud Hud { get; private set; }
		public CharacterMenu CharacterMenu { get; private set; }
		public CraftingMenu CraftingMenu { get; private set; }
		public BuildMenu BuildMenu { get; private set; }
		public MapMenu MapMenu { get; private set; }
		
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

			ConnectHudButtons();

			CloseMenu(Hud);
			CloseMenu(CharacterMenu);
			CloseMenu(CraftingMenu);
			CloseMenu(BuildMenu);
			CloseMenu(MapMenu);

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

        public void ConnectSignals(TeamData teamData)
        {
	        BuildMenu.ConnectSignals(teamData.Inventory);
	        CraftingMenu.ConnectSignals(teamData.Inventory);
	        CharacterMenu.ConnectSignals(teamData);
        }
		
        /// <summary>
        /// Only called once in beginning
        /// </summary>
		public void Initialize(TeamData teamData, Camera camera)
		{
			Hud.Initialize(teamData);
			CharacterMenu.Initialize(teamData);
			CraftingMenu.Initialize(teamData);
			MapMenu.Initialize(teamData);
			BuildMenu.Initialize(teamData, camera);
		}

        /// <summary>
        /// Called each time from Game.SetupLevel().
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
			CloseMenu(CharacterMenu);
			CloseMenu(CraftingMenu);
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
	}
}
