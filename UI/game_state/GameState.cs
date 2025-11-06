using Godot;
using System;

namespace Lastdew
{
    public abstract class GameState
    {
        public event Action OnToggleMain;
        public event Action OnToggleGame;
        public event Action OnToggleBuild;
        public event Action OnToggleMap;

        public abstract void EnterState(MainMenu mainMenu);
        public abstract void HandleInput(InputEvent @event);
        
        protected void ToggleMainMenu()
        {
            OnToggleMain?.Invoke();
        }
        
        protected void ToggleGameMenu()
        {
            OnToggleGame?.Invoke();
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
