#if TOOLS
using Godot;
using System;

namespace Lastdew
{
    [Tool]
    public partial class EditResourcePopup : CenterContainer
    {
        private Craftable Original { get; set; }
        private Craftable Copy { get; set; }
        private IconButton IconButton { get; set; }
    
        public override void _Ready()
        {
            base._Ready();

            IconButton.OnSetIcon += SetIcon;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            IconButton.OnSetIcon -= SetIcon;
        }

        public override void _GuiInput(InputEvent @event)
        {
            base._GuiInput(@event);
            
            if (@event is InputEventMouseButton button && button.ButtonIndex == MouseButton.Left && button.Pressed)
            {
                Hide();
            }
        }

        // TODO: How to handle new craftables? Have to hide/show the correct property displays.
        public void Setup(Craftable craftable)
        {
            Original = craftable;
            // Make a copy of the resource to edit/mess with until you click save or cancel.
            Copy = (Craftable)craftable.Duplicate();
            // TODO: Have all the property displays send signals when they change,
            // and then change the copy craftable accordingly.
            // Then save the copy back to the original when you press save.
        
            // TODO: Set all general craftable properties in the displays
            
            // TODO: Set all specific properties, and hide/show appropriate displays
            switch (craftable)
            {
                case Building building:
                    break;
                case CraftingMaterial craftingMaterial:
                    break;
                case Equipment equipment:
                    break;
                case UsableItem usableItem:
                    break;
                default:
                    break;
            }
        }
        
        private void SetIcon(Texture2D texture2D)
        {
            throw new NotImplementedException();
        }
    }
}
#endif
