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

            foreach (Craftable craftable in Databases.CRAFTABLES.Materials.Values)
            {
                Craftables.Add(craftable);
            }
            //Craftables = (ICollection<Craftable>)Databases.CRAFTABLES.Materials.Values.Cast<Craftable>();
            CraftableDisplayScene = GD.Load<PackedScene>(UIDs.CRAFTING_MATERIAL_DISPLAY);
        }
        
        protected override void CreateNewCraftable()
        {
            CraftingMaterial craftingMaterial = new();
            
            // TODO: Finish setting up craftable. Then save resource? Needed to get UID?
            
            // TODO: Add to Craftables database.
            
            // TODO: Open resource editor popup with new craftable.
            
        }
	}
}
#endif
