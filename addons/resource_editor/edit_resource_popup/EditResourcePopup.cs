#if TOOLS
using System;
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class EditResourcePopup : CenterContainer
    {
        public event Action<long> OnSaveCraftable;

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
            NameEdit = GetNode<LineEdit>("%NameEdit");
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
            
            if (@event.IsLeftClick())
            {
                Hide();
            }
        }

        public void Setup(Craftable craftable)
        {
            Craftable = craftable;
            ClearColumns();
            // TODO: Instantiate each property UI as needed. 
            StatsToCraftEdit statsToCraft = (StatsToCraftEdit)StatsToCraftScene.Instantiate();
            Column1.AddChild(statsToCraft);
            
            // Initialize property UIs
            IconButton.Icon = craftable.Icon;
            NameEdit.Text = craftable.Name;
            DescriptionEdit.Text = craftable.Description;
            // Using Get() to get private properties.
            // TODO: Change craftable properties to be string/int dicts instead, and handle all the conversion here,
            // and in custom inspectors. 
            statsToCraft.Setup((Godot.Collections.Dictionary<StatType, int>)craftable.Get(Craftable.PropertyName.StatsNeededToCraft));
            
            // TODO: Instantiate/initialize all subclass-specific properties
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
        
        private void ClearColumns()
        {
            foreach (Node child in Column1.GetChildren())
            {
                child.QueueFree();
            }
            foreach (Node child in Column2.GetChildren())
            {
                child.QueueFree();
            }
        }
        
        private void Save()
        {
            Craftable.Set(Craftable.PropertyName.Name, NameEdit.Text);
            Craftable.Set(Craftable.PropertyName.Description, DescriptionEdit.Text);
            Craftable.Set(Craftable.PropertyName.Icon, IconButton.Icon);
            // TODO: For each property UI, get the data from it and save it to the resource. 
            foreach (Node node in GetChildren())
            {
                if (node is IPropertyUi propertyUi)
                {
                    propertyUi.Save(Craftable);
                }
            }

            OnSaveCraftable?.Invoke(Extensions.GetUid(Craftable));
        }
    }
}
#endif
