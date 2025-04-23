#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class RequiredBuildingsDisplay : IconsDisplay
    {
        protected override void PopulateIcons(Craftable craftable)
        {
            foreach (long building in craftable.RequiredBuildings)
            {
                CraftableIcon icon = (CraftableIcon)CraftableIconScene.Instantiate();
                IconsParent.AddChild(icon);
                icon.Setup(Craftables[building], "");
            }
        }
    }
}
#endif
