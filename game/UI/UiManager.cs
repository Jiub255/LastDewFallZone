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
		
		public override void _Ready()
		{
			MainMenu = GetNode<MainMenu>("%MainMenu");
			Hud = GetNode<Hud>("%HUD");
			GameMenu = GetNode<GameMenu>("%GameMenu");
			BuildMenu = GetNode<BuildMenu>("%BuildMenu");
			MapMenu = GetNode<MapMenu>("%MapMenu");
		}
	
		public override void _Input(InputEvent @event)
		{
			//this.PrintDebug($"Event: {@event}");
			base._Input(@event);
			
			if (AnyMenuOpen && @event.IsActionPressed(InputNames.EXIT_MENU))
			{
				CloseAllMenus();
				return;
			}
			
			if (@event.IsActionPressed(InputNames.GAME_MENU))
			{
				this.PrintDebug($"Game menu pressed");
				Toggle(GameMenu);
			}
			else if (@event.IsActionPressed(InputNames.BUILD_MENU))
			{
				Toggle(BuildMenu);
			}
			else if (@event.IsActionPressed(InputNames.MAP_MENU))
			{
				Toggle(MapMenu);
			}
			else if (@event.IsActionPressed(InputNames.MAIN_MENU))
			{
				Toggle(MainMenu);
			}
		}
		
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			Hud.Initialize(teamData);
			GameMenu.Initialize(teamData, inventoryManager);

			MainMenu.Close();
			Hud.Open();
			
			GetTree().Paused = false;
		}
		
		private void Toggle(Menu menu)
		{
			if (menu.Visible)
			{
				menu.Close();
				Hud.Open();
				AnyMenuOpen = false;
				GetTree().Paused = false;
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
	}
}
