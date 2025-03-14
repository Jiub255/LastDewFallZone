using Godot;

namespace Lastdew
{
    public class GameStateScavenging : GameState
    {
        public GameStateScavenging() : base() {}
    
        public override void EnterState(MainMenu mainMenu)
        {
            mainMenu.Continue.Hide();
            mainMenu.SaveGame.Hide();
            mainMenu.ReturnToBase.Show();
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
        }
    }
}
