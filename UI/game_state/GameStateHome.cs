using Godot;

namespace Lastdew
{
    public class GameStateHome : GameState
    {
	    public override void EnterState(MainMenu mainMenu)
        {
            mainMenu.Continue.Hide();
            mainMenu.SaveGame.Show();
            mainMenu.ReturnToBase.Hide();
            mainMenu.LoadGame.Show();
            mainMenu.NewGame.Hide();
            mainMenu.Exit.InStartMenu = false;
        }

        public override void ProcessState()
        {
	        if (Input.IsActionJustReleased(InputNames.GAME_MENU))
	        {
		        ToggleGameMenu();
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
