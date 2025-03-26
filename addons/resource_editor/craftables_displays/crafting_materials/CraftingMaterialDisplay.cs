#if TOOLS
using System;
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class CraftingMaterialDisplay : CraftableDisplay
    {
      /*   public event Action<CraftingMaterial> OnOpenPopupPressed;
        
        private CraftingMaterial CraftingMaterial { get; set; } */
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
          //  CraftingMaterial = craftingMaterial;
            Reusable.ButtonPressed = craftingMaterial.Reusable;
        }

    /*     protected override void OpenEditPopup()
        {
            OnOpenPopupPressed?.Invoke(CraftingMaterial);
        } */
    }
}
#endif
