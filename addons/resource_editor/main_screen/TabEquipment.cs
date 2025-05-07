#if TOOLS
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class TabEquipment : TabCraftable
	{
        public override void _Ready()
        {
            base._Ready();

            foreach (Craftable craftable in Databases.CRAFTABLES.Equipment.Values)
            {
                Craftables.Add(craftable);
            }
            CraftableDisplayScene = GD.Load<PackedScene>(UIDs.EQUIPMENT_RESOURCE_DISPLAY);
           // Path = "res://craftables/items/equipment/";
        }
        
        protected override void CreateNewCraftable()
        {
            Equipment equipment = new();
            
            // TODO: Get filename from popup window before saving. 
            // OR, just don't save the resource until pressing save (with name field filled out and not already in use)
          //  ResourceSaver.Save(equipment, Path);

          //  long uid = ResourceLoader.GetResourceUid(Path);
           // Databases.CRAFTABLES.Equipment[uid] = equipment;
            
            OpenPopup(equipment);
        }
	}
}
#endif
