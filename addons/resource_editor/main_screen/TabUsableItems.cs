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

            foreach (Craftable craftable in Databases.CRAFTABLES.UsableItems.Values)
            {
                Craftables.Add(craftable);
            }
            //Craftables = (ICollection<Craftable>)Databases.CRAFTABLES.UsableItems.Values.Cast<Craftable>();
            CraftableDisplayScene = GD.Load<PackedScene>(UIDs.USABLE_ITEM_DISPLAY);
        }
        
        protected override void CreateNewCraftable()
        {
            UsableItem usableItem = new();
            
            // TODO: Finish setting up craftable. Then save resource? Needed to get UID?
            
            // TODO: Add to Craftables database.
            
            // TODO: Open resource editor popup with new craftable.
            
        }
	}
}
#endif
