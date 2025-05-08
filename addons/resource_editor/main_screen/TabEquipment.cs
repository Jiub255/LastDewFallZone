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

            foreach (Craftable craftable in Databases.CRAFTABLES.Equipments.Values)
            {
                Craftables.Add(craftable);
            }
            CraftableDisplayScene = GD.Load<PackedScene>(UIDs.EQUIPMENT_RESOURCE_DISPLAY);
        }
        
        protected override void CreateNewCraftable()
        {
            Equipment equipment = new();
            OpenPopup(equipment);
        }
	}
}
#endif
