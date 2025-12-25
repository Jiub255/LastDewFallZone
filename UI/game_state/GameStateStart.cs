namespace Lastdew
{
    public class GameStateStart : GameState
    {
        public override void EnterState(UiManager uiManager)
        {
            uiManager.MainMenu.Continue.Show();
            uiManager.MainMenu.SaveGame.Hide();
            uiManager.MainMenu.LoadGame.Show();
            uiManager.MainMenu.NewGame.Show();
            uiManager.MainMenu.Options.Show();
            uiManager.MainMenu.Exit.InStartMenu = true;

            uiManager.Hud.Build.Hide();
            uiManager.Hud.Craft.Hide();
            uiManager.Hud.Character.Hide();
            uiManager.Hud.Map.Hide();
            uiManager.Hud.Main.Hide();
            uiManager.Hud.ReturnHome.Hide();
        }

        public override void ProcessState() {}
    }
}
