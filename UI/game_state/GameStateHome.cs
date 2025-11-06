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

        public override void HandleInput(InputEvent @event)
        {
            if (@event.IsActionPressed(InputNames.GAME_MENU))
			{
                ToggleGameMenu();
			}
			else if (@event.IsActionPressed(InputNames.MAIN_MENU))
			{
                ToggleMainMenu();
			}
			else if (@event.IsActionPressed(InputNames.BUILD_MENU))
			{
                ToggleBuildMenu();
			}
			else if (@event.IsActionPressed(InputNames.MAP_MENU))
			{
                ToggleMapMenu();
			}
        }
    }
}
