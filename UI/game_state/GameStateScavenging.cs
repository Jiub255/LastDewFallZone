using Godot;

namespace Lastdew
{
    public class GameStateScavenging : GameState
    {
        public override void EnterState(UiManager uiManager)
        {
            uiManager.MainMenu.Continue.Hide();
            uiManager.MainMenu.SaveGame.Hide();
            uiManager.MainMenu.LoadGame.Show();
            uiManager.MainMenu.NewGame.Hide();
            uiManager.MainMenu.Options.Show();
            uiManager.MainMenu.Exit.InStartMenu = false;

            uiManager.Hud.Build.Hide();
            uiManager.Hud.Craft.Hide();
            uiManager.Hud.Character.Hide();
            uiManager.Hud.Map.Hide();
            uiManager.Hud.Main.Show();
            uiManager.Hud.ReturnHome.Show();
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
