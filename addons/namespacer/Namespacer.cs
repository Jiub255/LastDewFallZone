#if TOOLS

using Godot;

namespace Lastdew
{
    [Tool]
    public partial class Namespacer : EditorPlugin
    {
        private NamespaceContextMenu Plugin { get; set; }
        
        public override void _EnterTree()
        {
            base._EnterTree();

            Plugin = new NamespaceContextMenu();
            AddContextMenuPlugin(EditorContextMenuPlugin.ContextMenuSlot.Filesystem, Plugin);
        }

        public override void _ExitTree()
        {
            base._ExitTree();

            RemoveContextMenuPlugin(Plugin);
        }
    }
}
#endif
