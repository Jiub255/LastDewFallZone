#if TOOLS
using Godot;
using System.Collections.Generic;

namespace Lastdew
{
    [Tool]
    public partial class ScrapResultsDisplay : IconsDisplay
    {
        protected override void PopulateIcons(Craftable craftable)
        {
            foreach (KeyValuePair<long, int> kvp in craftable.ScrapResults)
            {
                CraftableIcon icon = (CraftableIcon)CraftableIconScene.Instantiate();
                IconsParent.AddChild(icon);
                icon.Setup(Databases.CRAFTABLES[kvp.Key], kvp.Value.ToString());
            }
        }
    }
}
#endif
