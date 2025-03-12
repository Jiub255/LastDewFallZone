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
    
        public GameState() {}
    
        public abstract void EnterState(MainMenu mainMenu);
        public abstract void HandleInput(InputEvent @event);
        
        protected void ToggleMain()
        {
            OnToggleMain?.Invoke();
        }
        
        protected void ToggleGame()
        {
            OnToggleGame?.Invoke();
        }
        
        protected void ToggleBuild()
        {
            OnToggleBuild?.Invoke();
        }
        
        protected void ToggleMap()
        {
            OnToggleMap?.Invoke();
        }
    }
}
