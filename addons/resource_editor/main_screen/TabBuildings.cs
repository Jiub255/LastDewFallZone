#if TOOLS
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class TabBuildings : TabCraftable
	{
        public override void _Ready()
        {
            base._Ready();

            foreach (Craftable craftable in Databases.CRAFTABLES.Buildings.Values)
            {
                Craftables.Add(craftable);
            }
            //Craftables = (ICollection<Craftable>)Databases.CRAFTABLES.Buildings.Values.Cast<Craftable>();
            CraftableDisplayScene = GD.Load<PackedScene>(UIDs.BUILDING_DISPLAY);
        }
        
        protected override void CreateNewCraftable()
        {
            Building building = new();
            
            // TODO: Finish setting up craftable. Then save resource? Needed to get UID?
            
            // TODO: Add to Craftables database.
            
            // TODO: Open resource editor popup with new craftable.
            
        }
	}
}
#endif
