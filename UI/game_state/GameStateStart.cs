using Godot;

namespace Lastdew
{
    public class GameStateStart : GameState
    {
        public override void EnterState(MainMenu mainMenu)
        {
            mainMenu.Continue.Show();
            mainMenu.SaveGame.Hide();
            mainMenu.ReturnToBaseButton.Hide();
            mainMenu.LoadGame.Show();
            mainMenu.NewGame.Show();
            mainMenu.Exit.InStartMenu = true;
        }

        public override void ProcessState() {}
    }
}
