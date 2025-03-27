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
        private Callable SetIconCallable { get; set; }
    
        public override void _Ready()
        {
            base._Ready();
        
            SetIconCallable = new Callable(this, MethodName.SetIcon);
            
            Pressed += ChooseIcon;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            Pressed -= ChooseIcon;
        }
        
        private void ChooseIcon()
        {
            EditorInterface.PopupQuickOpen(SetIconCallable, ["Texture2D"]);
        }

        private void SetIcon(string path)
        {
            OnSetIcon?.Invoke(GD.Load<Texture2D>(path));
        }
    }
}
#endif
