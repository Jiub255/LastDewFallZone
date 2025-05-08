#if TOOLS
using System;
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class UsableItemsDisplay : ItemDisplay
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
            
            UsableItem usableItem = craftable as UsableItem;
            Reusable.ButtonPressed = usableItem.Reusable;
        }
    }
}
#endif
