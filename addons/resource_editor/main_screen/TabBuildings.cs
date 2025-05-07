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
        }
        
        protected override void CreateNewCraftable()
        {
            Building building = new();
            OpenPopup(building);
        }
	}
}
#endif
