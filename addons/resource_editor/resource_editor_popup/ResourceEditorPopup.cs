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
            scrapResults.Setup(craftable.ScrapResults);
            
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
                if (node is IPropertyEditor propertyEditor)
                {
                    propertyEditor.Save(Craftable);
                }
            }
            
            Error error = new();
            this.PrintDebug($"Resource path: {Craftable.ResourcePath}");
            if (Craftable.ResourcePath == "")
            {
                if (Craftable.Name == "")
                {
                    // TODO: Popup saying name can't be empty.
                    GD.PushError($"New {Craftable.GetType()} name can't be empty");
                    return;
                }
                switch (Craftable)
                {
                    case CraftingMaterial:
                        string material_path = $"res://craftables/items/crafting-materials/{Craftable.Name.ToSnakeCase()}.tres";
                        if (ResourceLoader.Exists(material_path))
                        {
                            // TODO: Popup saying name already taken.
                            GD.PushError($"Resource already exists at {material_path}");
                            return;
                        }
                        error = ResourceSaver.Save(Craftable, material_path);
                        if (error == Error.Ok)
                        {
                            Craftable = ResourceLoader.Load<CraftingMaterial>(material_path);
                            Databases.CRAFTABLES.CraftingMaterials[Craftable.GetUid()] = (CraftingMaterial)Craftable;
                            this.PrintDebug($"Saved {Craftable.Name} to {material_path}");
                        }
                        else
                        {
                            GD.PushError($"Craftable {Craftable.Name} save failed. {error}");
                        }
                        break;
                    case Equipment:
                        string equipment_path = $"res://craftables/items/equipment/{Craftable.Name.ToSnakeCase()}.tres";
                        if (ResourceLoader.Exists(equipment_path))
                        {
                            // TODO: Popup saying name already taken.
                            GD.PushError($"Resource already exists at {equipment_path}");
                            return;
                        }
                        error = ResourceSaver.Save(Craftable, equipment_path);
                        if (error == Error.Ok)
                        {
                            Craftable = ResourceLoader.Load<Equipment>(equipment_path);
                            Databases.CRAFTABLES.Equipment[Craftable.GetUid()] = (Equipment)Craftable;
                            this.PrintDebug($"Saved {Craftable.Name} to {equipment_path}");
                        }
                        else
                        {
                            GD.PushError($"Craftable {Craftable.Name} save failed. {error}");
                        }
                        break;
                    case UsableItem:
                        string usable_item_path = $"res://craftables/items/usable-items/{Craftable.Name.ToSnakeCase()}.tres";
                        if (ResourceLoader.Exists(usable_item_path))
                        {
                            // TODO: Popup saying name already taken.
                            GD.PushError($"Resource already exists at {usable_item_path}");
                            return;
                        }
                        error = ResourceSaver.Save(Craftable, usable_item_path);
                        if (error == Error.Ok)
                        {
                            Craftable = ResourceLoader.Load<UsableItem>(usable_item_path);
                            Databases.CRAFTABLES.UsableItems[Craftable.GetUid()] = (UsableItem)Craftable;
                            this.PrintDebug($"Saved {Craftable.Name} to {usable_item_path}");
                        }
                        else
                        {
                            GD.PushError($"Craftable {Craftable.Name} save failed. {error}");
                        }
                        break;
                    case Building:
                        string building_path = $"res://craftables/bulidings/{Craftable.Name.ToSnakeCase()}.tres";
                        if (ResourceLoader.Exists(building_path))
                        {
                            // TODO: Popup saying name already taken.
                            GD.PushError($"Resource already exists at {building_path}");
                            return;
                        }
                        error = ResourceSaver.Save(Craftable, building_path);
                        if (error == Error.Ok)
                        {
                            Craftable = ResourceLoader.Load<Building>(building_path);
                            Databases.CRAFTABLES.Buildings[Craftable.GetUid()] = (Building)Craftable;
                            this.PrintDebug($"Saved {Craftable.Name} to {building_path}");
                        }
                        else
                        {
                            GD.PushError($"Craftable {Craftable.Name} save failed. {error}");
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                error = ResourceSaver.Save(Craftable);
            }
            if (error != Error.Ok)
            {
                GD.PushError($"Craftable {Craftable.Name} save failed. {error}");
            }

            // TODO: Refresh tab with new CraftableDisplay. Use this event?
            OnSaveCraftable?.Invoke(Craftable.GetUid());
            Hide();
        }
    }
}
#endif
