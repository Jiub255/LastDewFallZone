using Godot;

namespace Lastdew
{
    public class GameStateScavenging : GameState
    {
        public override void EnterState(MainMenu mainMenu)
        {
            mainMenu.Continue.Hide();
            mainMenu.SaveGame.Hide();
            mainMenu.ReturnToBase.Show();
            mainMenu.LoadGame.Show();
            mainMenu.NewGame.Hide();
            mainMenu.Exit.InStartMenu = false;
        }

        public override void ProcessState()
        {
            if (Input.IsActionJustReleased(InputNames.MAIN_MENU))
            {
                ToggleMainMenu();
            }
        }
    }
}
