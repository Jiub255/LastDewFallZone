#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public abstract partial class IconsDisplay : PanelContainer
    {
        protected PackedScene CraftableIconScene { get; } = GD.Load<PackedScene>(Uids.CRAFTABLE_ICON);
        protected HBoxContainer IconsParent { get; set; }


        public override void _Ready()
        {
            base._Ready();
            
            IconsParent = GetNode<HBoxContainer>("%IconsParent");
        }

        public void Setup(Craftable craftable)
        {
            ClearIcons();
            PopulateIcons(craftable);
        }

        protected abstract void PopulateIcons(Craftable craftable);

        private void ClearIcons()
        {
            foreach (Node node in IconsParent.GetChildren())
            {
                if (node is CraftableIcon icon)
                {
                    icon.QueueFree();
                }
            }
        }
    }
}
#endif
