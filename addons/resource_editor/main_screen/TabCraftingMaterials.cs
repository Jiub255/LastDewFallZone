 #if TOOLS
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class TabCraftingMaterials : TabCraftable
	{
        public override void _Ready()
        {
            base._Ready();

            foreach (Craftable craftable in Databases.CRAFTABLES.CraftingMaterials.Values)
            {
                Craftables.Add(craftable);
            }
            CraftableDisplayScene = GD.Load<PackedScene>(UIDs.CRAFTING_MATERIAL_DISPLAY);
        }
        
        protected override void CreateNewCraftable()
        {
            CraftingMaterial craftingMaterial = new();
            OpenPopup(craftingMaterial);
        }
	}
}
#endif
