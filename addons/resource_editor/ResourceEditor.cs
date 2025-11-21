#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class ResourceEditor : EditorPlugin
    {
        private PackedScene MainScreenScene { get; } = GD.Load<PackedScene>(Uids.EDITOR_MAIN_SCREEN);
        private EditorMainScreen MainScreenInstance { get; set; }
        private EditorInterface EditorInterface { get; } = EditorInterface.Singleton;
        private EditorFileDialog EditorFileDialog { get; set; }

        public override void _EnterTree()
        {
            MainScreenInstance = (EditorMainScreen)MainScreenScene.Instantiate();
            EditorInterface.GetEditorMainScreen().AddChild(MainScreenInstance);
            _MakeVisible(false);
        }

        public override void _ExitTree()
        {
            if (MainScreenInstance != null)
            {
                MainScreenInstance.QueueFree();
            }
        }

        public override bool _HasMainScreen()
        {
            return true;
        }

        public override void _MakeVisible(bool visible)
        {
            if (MainScreenInstance != null)
            {
                MainScreenInstance.Visible = visible;
            }
        }

        public override string _GetPluginName()
        {
            return "Resource Editor";
        }

        public override Texture2D _GetPluginIcon()
        {
            return EditorInterface.GetEditorTheme().GetIcon("VBoxContainer", "EditorIcons");
        }
    }
}
#endif
