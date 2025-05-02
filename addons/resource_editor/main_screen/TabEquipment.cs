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
            //Craftables = (ICollection<Craftable>)Databases.CRAFTABLES.Equipment.Values.Cast<Craftable>();
            CraftableDisplayScene = GD.Load<PackedScene>(UIDs.EQUIPMENT_RESOURCE_DISPLAY);
        }
        
        protected override void CreateNewCraftable()
        {
            Equipment equipment = new();
            
            // TODO: Finish setting up craftable. Then save resource? Needed to get UID?
            
            // TODO: Add to Craftables database.
            
            // TODO: Open resource editor popup with new craftable.
            
        }
	}
}
#endif
