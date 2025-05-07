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
            CraftableDisplayScene = GD.Load<PackedScene>(UIDs.USABLE_ITEM_DISPLAY);
          //  Path = "res://craftables/items/usable-items/";
        }
        
        protected override void CreateNewCraftable()
        {
            UsableItem usableItem = new();
            
            // TODO: Get filename from popup window before saving. 
            // OR, just don't save the resource until pressing save (with name field filled out and not already in use)
           // ResourceSaver.Save(usableItem, Path);

           // long uid = ResourceLoader.GetResourceUid(Path);
           // Databases.CRAFTABLES.UsableItems[uid] = usableItem;
            
            OpenPopup(usableItem);
        }
	}
}
#endif
