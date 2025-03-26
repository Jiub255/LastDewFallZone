#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public abstract partial class IconsDisplay : PanelContainer
    {
        protected Craftables Craftables { get; } = GD.Load<Craftables>(UIDs.CRAFTABLES);
        protected PackedScene CraftableIconScene { get; } = GD.Load<PackedScene>(UIDs.CRAFTABLE_ICON);
        protected HBoxContainer IconsParent { get; set; }


        public override void _Ready()
        {
            base._Ready();
            
            IconsParent = GetNode<HBoxContainer>("%IconsParent");
        }

        public void Setup(Craftable craftable)
        {
            ClearIcons();
            if (craftable.RecipeCosts.Count == 0)
            {
                return;
            }
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
