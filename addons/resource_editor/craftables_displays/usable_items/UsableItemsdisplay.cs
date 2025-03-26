#if TOOLS
using System;
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class UsableItemsdisplay : CraftableDisplay
    {
        /* public event Action<UsableItem> OnOpenPopupPressed;
        
        private UsableItem UsableItem { get; set; } */
        private CheckBox Reusable { get; set; }
        private EffectsDisplay Effects { get; set; }

        public override void _Ready()
        {
            base._Ready();
            
            Reusable = GetNode<CheckBox>("%Reusable");
            Effects = GetNode<EffectsDisplay>("%Effects");
        }

        public override void Setup(Craftable craftable)
        {
            base.Setup(craftable);
            
            UsableItem usableItem = craftable as UsableItem;
           // UsableItem = usableItem;
            Reusable.ButtonPressed = usableItem.Reusable;
        }

     /*    protected override void OpenEditPopup()
        {
            OnOpenPopupPressed?.Invoke(UsableItem);
        } */
    }
}
#endif
