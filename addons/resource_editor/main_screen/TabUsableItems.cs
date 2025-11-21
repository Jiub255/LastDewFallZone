#if TOOLS
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class TabUsableItems : TabCraftable
	{
        public override void _Ready()
        {
            base._Ready();

            foreach (Craftable craftable in Databases.Craftables.UsableItems.Values)
            {
                Craftables.Add(craftable);
            }
            CraftableDisplayScene = GD.Load<PackedScene>(Uids.USABLE_ITEM_DISPLAY);
        }
        
        protected override void CreateNewCraftable()
        {
            UsableItem usableItem = new();
            OpenPopup(usableItem);
        }
	}
}
#endif
