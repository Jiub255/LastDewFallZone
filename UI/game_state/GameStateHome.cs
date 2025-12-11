using Godot;

namespace Lastdew
{
    public class GameStateHome : GameState
    {
	    public override void EnterState(MainMenu mainMenu)
        {
            mainMenu.Continue.Hide();
            mainMenu.SaveGame.Show();
            mainMenu.ReturnToBaseButton.Hide();
            mainMenu.LoadGame.Show();
            mainMenu.NewGame.Hide();
            mainMenu.Exit.InStartMenu = false;
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
		        this.PrintDebug($"Map menu pressed");
		        ToggleMapMenu();
	        }
        }
    }
}
