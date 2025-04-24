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
        private PackedScene StatsToCraftScene { get; } = GD.Load<PackedScene>(UIDs.STATS_TO_CRAFT);
        private PackedScene RecipeCostsScene { get; } = GD.Load<PackedScene>(UIDs.RECIPE_COSTS_EDIT);
        private PackedScene RequiredBuildingsScene { get; } = GD.Load<PackedScene>(UIDs.REQUIRED_BUILDINGS_EDIT);


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
            RecipeCostsEdit recipeCosts = (RecipeCostsEdit)RecipeCostsScene.Instantiate();
            Column1.AddChild(recipeCosts);

            StatsToCraftEdit statsToCraft = (StatsToCraftEdit)StatsToCraftScene.Instantiate();
            Column2.AddChild(statsToCraft);

            RequiredBuildingsEdit requiredBuildings = (RequiredBuildingsEdit)RequiredBuildingsScene.Instantiate();
            Column1.AddChild(requiredBuildings);
            
            // Initialize property UIs
            IconButton.Icon = craftable.Icon;
            NameEdit.Text = craftable.Name;
            DescriptionEdit.Text = craftable.Description;
            
            // TODO: Change craftable properties to be long(UID)/int(Amount) dicts instead, and handle all the conversion here,
            // and in custom inspectors. 
            // Using Get() to get private properties.
            statsToCraft.Setup((Godot.Collections.Dictionary<StatType, int>)craftable.Get(Craftable.PropertyName.StatsNeededToCraft));
            recipeCosts.Setup((Godot.Collections.Dictionary<CraftingMaterial, int>)craftable.Get(Craftable.PropertyName._recipeCosts));
            requiredBuildings.Setup((Godot.Collections.Array<Building>)craftable.Get(Craftable.PropertyName._requiredBuildings));
            
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
            foreach (Node node in this.GetChildrenRecursive())
            {
                this.PrintDebug($"Node: {node.Name}");
                if (node is IPropertyUi propertyUi)
                {
                    this.PrintDebug($"IPropertyUi: {node.Name}");
                    propertyUi.Save(Craftable);
                }
            }

            OnSaveCraftable?.Invoke(Craftable.GetUid());
            Hide();
        }
    }
}
#endif
