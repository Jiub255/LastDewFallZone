#if TOOLS
using Godot;
using System;
    
namespace Lastdew    
{
    [Tool]
    public partial class IconButton : Button
    {
        public event Action<Texture2D> OnSetIcon;
        
        private EditorInterface EditorInterface { get; } = EditorInterface.Singleton;
    
        public override void _Ready()
        {
            base._Ready();
            
            Pressed += ChooseIcon;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            Pressed -= ChooseIcon;
        }
        
        private void ChooseIcon()
        {
            // THIS IS HOW TO DO A QUICK LOAD DIALOG!!!!!!!!!!!!!!!!
            EditorInterface.PopupQuickOpen(new Callable(this, MethodName.SetIcon), ["Texture2D"]);
        }

        private void SetIcon(string path)
        {
            OnSetIcon?.Invoke(GD.Load<Texture2D>(path));
        }
    }
}
#endif
