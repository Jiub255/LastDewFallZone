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
            //Path = "res://craftables/items/crafting-materials/";
        }
        
        protected override void CreateNewCraftable()
        {
            CraftingMaterial craftingMaterial = new();

            // TODO: Get filename from popup window before saving. 
            // OR, just don't save the resource until pressing save (with name field filled out and not already in use)
           // ResourceSaver.Save(craftingMaterial, Path);

          //  long uid = ResourceLoader.GetResourceUid(Path);
           // Databases.CRAFTABLES.CraftingMaterials[uid] = craftingMaterial;
            
            OpenPopup(craftingMaterial);
        }
	}
}
#endif
