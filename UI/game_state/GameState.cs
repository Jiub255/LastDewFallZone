using System;

namespace Lastdew
{
    public abstract class GameState
    {
        public event Action OnToggleMain;
        public event Action OnToggleCharacter;
        public event Action OnToggleCrafting;
        public event Action OnToggleBuild;
        public event Action OnToggleMap;

        public abstract void EnterState(UiManager uiManager);
        public abstract void ProcessState();
        
        protected void ToggleMainMenu()
        {
            OnToggleMain?.Invoke();
        }
        
        protected void ToggleCharacterMenu()
        {
            OnToggleCharacter?.Invoke();
        }
        
        protected void ToggleCraftingMenu()
        {
            OnToggleCrafting?.Invoke();
        }
        
        protected void ToggleBuildMenu()
        {
            OnToggleBuild?.Invoke();
        }
        
        protected void ToggleMapMenu()
        {
            OnToggleMap?.Invoke();
        }
    }
}
