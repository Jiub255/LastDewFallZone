#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class CraftingMaterialDisplay : CraftableDisplay
    {
        private CheckBox Reusable { get; set; }

        public override void _Ready()
        {
            base._Ready();
            
            Reusable = GetNode<CheckBox>("%Reusable");
        }

        public override void Setup(Craftable craftable)
        {
            base.Setup(craftable);
            
            CraftingMaterial craftingMaterial = craftable as CraftingMaterial;
            Reusable.ButtonPressed = craftingMaterial.Reusable;
        }
    }
}
#endif
