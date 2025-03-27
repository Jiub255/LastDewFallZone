#if TOOLS
using Godot;
using System;

namespace Lastdew
{
    [Tool]
    public partial class EditResourcePopup : CenterContainer
    {
        private const string NAME = "Name";
        private const string DESCRIPTION = "Description";
        private const string ICON = "Icon";
        private const string RECIPE_COSTS = "_recipeCosts";
        private const string REQUIRED_BUILDINGS = "_requiredBuildings";
        private const string SCRAP_RESULTS = "_scrapResults";
        private const string STATS_TO_CRAFT = "StatsNeededToCraft";
    
        private Craftable Original { get; set; }
        private Craftable Copy { get; set; }
        
        private IconButton IconButton { get; set; }
        private LineEdit NameLineEdit { get; set; }
        private TextEdit Description { get; set; }
        // TODO: Make custom types for the more complex properties. Add them here.
    
        public override void _Ready()
        {
            base._Ready();
            
            IconButton = GetNode<IconButton>("%IconButton");
            NameLineEdit = GetNode<LineEdit>("%NameLineEdit");
            Description = GetNode<TextEdit>("%DescriptionTextEdit");

            IconButton.OnSetIcon += SetIcon;
            NameLineEdit.TextChanged += SetName;
            Description.TextChanged += SetDescription;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            IconButton.OnSetIcon -= SetIcon;
            NameLineEdit.TextChanged -= SetName;
            Description.TextChanged -= SetDescription;
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
            
            // TODO: Do the Ready() stuff here?
            // Then can only GetNode/subscribe events for the needed properties.
            // Especially since eventually going to be editing PcDatas and other stuff w/ no similar properties.

            // TODO: Set all general craftable properties in the displays
            IconButton.Icon = Copy.Icon;
             
            
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
        
        private void SetIcon(Texture2D icon)
        {
            Copy.Set(ICON, icon);
        }
        
        private new void SetName(string name)
        {
            Copy.Set(NAME, name);
        }
        
        private void SetDescription()
        {
            Copy.Set(DESCRIPTION, Description.Text);
        }

        // TODO: The rest of the methods, like above. Extra steps for the dict ones obviously. 
        
    }
}
#endif
