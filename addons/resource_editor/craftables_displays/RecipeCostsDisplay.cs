#if TOOLS
using Godot;
using System.Collections.Generic;

namespace Lastdew
{
    [Tool]
    public partial class RecipeCostsDisplay : IconsDisplay
    {
        protected override void PopulateIcons(Craftable craftable)
        {
            foreach (KeyValuePair<string, int> kvp in craftable.RecipeCosts)
            {
                CraftableIcon icon = (CraftableIcon)CraftableIconScene.Instantiate();
                CraftingMaterial material = (CraftingMaterial)Craftables[kvp.Key];
                string amount = material.Reusable ? "" : kvp.Value.ToString();
                IconsParent.AddChild(icon);
                icon.Setup(material, amount);
            }
        }
    }
}
#endif
