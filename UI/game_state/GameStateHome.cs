using Godot;

namespace Lastdew
{
    public class GameStateHome : GameState
    {
        public GameStateHome() : base() {}
    
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
                ToggleGame();
			}
			else if (@event.IsActionPressed(InputNames.MAIN_MENU))
			{
                ToggleMain();
			}
			else if (@event.IsActionPressed(InputNames.BUILD_MENU))
			{
                ToggleBuild();
			}
			else if (@event.IsActionPressed(InputNames.MAP_MENU))
			{
                ToggleMap();
			}
        }
    }
}
