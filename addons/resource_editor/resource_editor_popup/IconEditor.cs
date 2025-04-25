#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class IconEditor : Button
    {
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
            Icon = GD.Load<Texture2D>(path);
        }
    }
}
#endif
