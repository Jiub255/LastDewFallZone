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
            CraftableDisplayScene = GD.Load<PackedScene>(UIDs.BUILDING_DISPLAY);
           // Path = "res://craftables/buildings/";
        }
        
        protected override void CreateNewCraftable()
        {
            Building building = new();
            
            // TODO: Get filename from popup window before saving. 
            // OR, just don't save the resource until pressing save (with name field filled out and not already in use)
           // ResourceSaver.Save(building, Path);

           // long uid = ResourceLoader.GetResourceUid(Path);
            //Databases.CRAFTABLES.Buildings[uid] = building;
            
            OpenPopup(building);
        }
	}
}
#endif
