#if TOOLS
using System;
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class ResourceEditorPopup : CenterContainer
    {
        public event Action<long> OnSaveCraftable;

        private Craftable Craftable { get; set; }
        private IconEditor IconButton { get; set; }
        private LineEdit NameEdit { get; set; }
        private TextEdit DescriptionEdit { get; set; }
        private Button SaveButton { get; set; }
        private VBoxContainer Column1 { get; set; }
        private VBoxContainer Column2 { get; set; }
        private PackedScene StatsToCraftScene { get; } = GD.Load<PackedScene>(UIDs.STATS_TO_CRAFT);
        private PackedScene RecipeCostsScene { get; } = GD.Load<PackedScene>(UIDs.RECIPE_COSTS_EDIT);
        private PackedScene RequiredBuildingsScene { get; } = GD.Load<PackedScene>(UIDs.REQUIRED_BUILDINGS_EDIT);
        private PackedScene ScrapResultsScene { get; } = GD.Load<PackedScene>(UIDs.SCRAP_RESULTS_EDIT);


        public override void _Ready()
        {
            base._Ready();

            IconButton = GetNode<IconEditor>("%IconButton");
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

        // Hides popup (without saving) when clicking outside of popup.
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
            
            // Instantiate property editors
            MaterialAmountsEditor recipeCosts = (MaterialAmountsEditor)RecipeCostsScene.Instantiate();
            Column1.AddChild(recipeCosts);

            StatsToCraftEditor statsToCraft = (StatsToCraftEditor)StatsToCraftScene.Instantiate();
            Column2.AddChild(statsToCraft);

            RequiredBuildingsEditor requiredBuildings = (RequiredBuildingsEditor)RequiredBuildingsScene.Instantiate();
            Column1.AddChild(requiredBuildings);

            ScrapResultsEditor scrapResults = (ScrapResultsEditor)ScrapResultsScene.Instantiate();
            Column2.AddChild(scrapResults);
            
            // Initialize property editors
            IconButton.Icon = craftable.Icon;
            NameEdit.Text = craftable.Name;
            DescriptionEdit.Text = craftable.Description;
            
            statsToCraft.Setup(craftable.StatsNeededToCraft);
            recipeCosts.Setup(craftable.RecipeCosts);
            requiredBuildings.Setup(craftable.RequiredBuildings);
            
            // TODO: Instantiate/initialize all subclass-specific property editors
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
            
            // For each property editor, get the data from it and set it to the resource. 
            foreach (Node node in this.GetChildrenRecursive())
            {
                //this.PrintDebug($"Node: {node.Name}");
                if (node is IPropertyEditor propertyEditor)
                {
                    //this.PrintDebug($"IPropertyUi: {node.Name}");
                    propertyEditor.Save(Craftable);
                }
            }

            Error error = ResourceSaver.Save(Craftable);
            if (error != Error.Ok)
            {
                GD.PushError($"Craftable {Craftable.Name} save failed. {error}");
            }

            OnSaveCraftable?.Invoke(Craftable.GetUid());
            Hide();
        }
    }
}
#endif
