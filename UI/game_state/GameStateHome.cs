using Godot;

namespace Lastdew
{
    public class GameStateHome : GameState
    {
	    public override void EnterState(UiManager uiManager)
        {
            uiManager.MainMenu.Continue.Hide();
            uiManager.MainMenu.SaveGame.Show();
            uiManager.MainMenu.LoadGame.Show();
            uiManager.MainMenu.NewGame.Hide();
            uiManager.MainMenu.Options.Show();
            uiManager.MainMenu.Exit.InStartMenu = false;

            uiManager.Hud.Build.Show();
            uiManager.Hud.Craft.Show();
            uiManager.Hud.Character.Show();
            uiManager.Hud.Map.Show();
            uiManager.Hud.Main.Show();
            uiManager.Hud.ReturnHome.Hide();
        }

        public override void ProcessState()
        {
	        if (Input.IsActionJustReleased(InputNames.CHARACTER_MENU))
	        {
		        ToggleCharacterMenu();
	        }
	        if (Input.IsActionJustReleased(InputNames.CRAFTING_MENU))
	        {
		        ToggleCraftingMenu();
	        }
	        else if (Input.IsActionJustReleased(InputNames.MAIN_MENU))
	        {
		        ToggleMainMenu();
	        }
	        else if (Input.IsActionJustReleased(InputNames.BUILD_MENU))
	        {
		        ToggleBuildMenu();
	        }
	        else if (Input.IsActionJustReleased(InputNames.MAP_MENU))
	        {
		        ToggleMapMenu();
	        }
        }
    }
}
