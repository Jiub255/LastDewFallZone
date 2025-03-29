#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class EditResourcePopup : CenterContainer
    {
        // TODO: Copy each of these to its respective UI class.
        private const string NAME = "Name";
        private const string DESCRIPTION = "Description";
        private const string ICON = "Icon";
        private const string RECIPE_COSTS = "_recipeCosts";
        private const string REQUIRED_BUILDINGS = "_requiredBuildings";
        private const string SCRAP_RESULTS = "_scrapResults";
        private const string STATS_TO_CRAFT = "StatsNeededToCraft";
    
        private Craftable Craftable { get; set; }
        private IconButton IconButton { get; set; }
        private LineEdit NameEdit { get; set; }
        private TextEdit DescriptionEdit { get; set; }
        private Button SaveButton { get; set; }
        private VBoxContainer Column1 { get; set; }
        private VBoxContainer Column2 { get; set; }
        private PackedScene IconButtonScene { get; } = GD.Load<PackedScene>(UIDs.ICON_BUTTON);
        private PackedScene StatsToCraftScene { get; } = GD.Load<PackedScene>(UIDs.STATS_TO_CRAFT);


        public override void _Ready()
        {
            base._Ready();

            IconButton = GetNode<IconButton>("%IconButton");
            NameEdit = GetNode<LineEdit>("NameEdit");
            DescriptionEdit = GetNode<TextEdit>("%DescriptionEdit");
            SaveButton = GetNode<Button>("%SaveButton");
            Column1 = GetNode<VBoxContainer>("%Column1");
            Column2 = GetNode<VBoxContainer>("%Column2");

            SaveButton.Pressed += Save;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            SaveButton.Pressed -= Save;
        }

        public override void _GuiInput(InputEvent @event)
        {
            base._GuiInput(@event);
            
            if (@event is InputEventMouseButton button && button.ButtonIndex == MouseButton.Left && button.Pressed)
            {
                Hide();
            }
        }

        public void Setup(Craftable craftable)
        {
            Craftable = craftable;
            // TODO: Instantiate each property UI as needed. 
            StatsToCraftUi statsToCraft = (StatsToCraftUi)StatsToCraftScene.Instantiate();
            Column1.AddChild(statsToCraft);
            
            // Initialize property UIs
            IconButton.Icon = craftable.Icon;
            NameEdit.Text = craftable.Name;
            DescriptionEdit.Text = craftable.Description;
            // Using Get() to get private properties.
            // TODO: Change craftable properties to be string/int dicts instead, and handle all the conversion here,
            // and in custom inspectors. 
            statsToCraft.Setup((Godot.Collections.Dictionary<Lastdew.StatType, int>)craftable.Get(STATS_TO_CRAFT));
            
            // TODO: Instantiate/initialize all specific properties
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
        
        private void Save()
        {
            Craftable.Set(NAME, NameEdit.Text);
            Craftable.Set(DESCRIPTION, DescriptionEdit.Text);
            Craftable.Set(ICON, IconButton.Icon);
            // TODO: For each property UI, get the data from it and save it to the resource. 
            foreach (Node node in GetChildren())
            {
                if (node is IPropertyUi propertyUi)
                {
                    propertyUi.Save(Craftable);
                }
            }
        }
    }
}
#endif
