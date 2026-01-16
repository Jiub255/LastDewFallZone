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
        private AcceptDialog AcceptDialog { get; set; }
        private VBoxContainer Column1 { get; set; }
        private VBoxContainer Column2 { get; set; }
        private PackedScene StatsToCraftScene { get; } = GD.Load<PackedScene>(Uids.STATS_TO_CRAFT_EDITOR);
        private PackedScene RecipeCostsScene { get; } = GD.Load<PackedScene>(Uids.RECIPE_COSTS_EDITOR);
        private PackedScene RequiredBuildingsScene { get; } = GD.Load<PackedScene>(Uids.REQUIRED_BUILDINGS_EDITOR);
        private PackedScene ScrapResultsScene { get; } = GD.Load<PackedScene>(Uids.SCRAP_RESULTS_EDITOR);
        private PackedScene RarityEditorScene { get; } = GD.Load<PackedScene>(Uids.RARITY_EDITOR);
        private PackedScene TagsEditorScene { get; } = GD.Load<PackedScene>(Uids.TAGS_EDITOR);
        private PackedScene ReusableEditorScene { get; } = GD.Load<PackedScene>(Uids.REUSABLE_EDITOR);
        private PackedScene EquipmentBonusesScene { get; } = GD.Load<PackedScene>(Uids.EQUIPMENT_BONUSES_EDITOR);
        private PackedScene EquipmentTypeScene { get; } = GD.Load<PackedScene>(Uids.EQUIPMENT_TYPE_EDITOR);
        private PackedScene StatsToEquipScene { get; } = GD.Load<PackedScene>(Uids.STATS_TO_EQUIP_EDITOR);
        private PackedScene BuildingTypeScene { get; } = GD.Load<PackedScene>(Uids.BUILDING_TYPE_EDITOR);
        private PackedScene SceneUidScene { get; } = GD.Load<PackedScene>(Uids.SCENE_UID_EDITOR);
        private PackedScene EffectsScene { get; } = GD.Load<PackedScene>(Uids.EFFECTS_EDITOR);


        public override void _Ready()
        {
            base._Ready();

            IconButton = GetNode<IconEditor>("%IconButton");
            NameEdit = GetNode<LineEdit>("%NameEdit");
            DescriptionEdit = GetNode<TextEdit>("%DescriptionEdit");
            SaveButton = GetNode<Button>("%SaveButton");
            AcceptDialog = GetNode<AcceptDialog>("%AcceptDialog");
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
            
            // Instantiate/Initialize property editors
            IconButton.Icon = craftable.Icon;
            NameEdit.Text = craftable.Name;
            DescriptionEdit.Text = craftable.Description;
            
            MaterialAmountsEditor recipeCosts = (MaterialAmountsEditor)RecipeCostsScene.Instantiate();
            Column1.AddChild(recipeCosts);
            recipeCosts.Setup(craftable.RecipeCosts);

            StatsToCraftEditor statsToCraft = (StatsToCraftEditor)StatsToCraftScene.Instantiate();
            Column2.AddChild(statsToCraft);
            statsToCraft.Setup(craftable.StatsNeededToCraft);

            RequiredBuildingsEditor requiredBuildings = (RequiredBuildingsEditor)RequiredBuildingsScene.Instantiate();
            Column1.AddChild(requiredBuildings);
            requiredBuildings.Setup(craftable.RequiredBuildings);

            ScrapResultsEditor scrapResults = (ScrapResultsEditor)ScrapResultsScene.Instantiate();
            Column2.AddChild(scrapResults);
            scrapResults.Setup(craftable.ScrapResults);
            
            if (craftable is Item item)
            {
                RarityEditor rarityEditor = (RarityEditor)RarityEditorScene.Instantiate();
                Column1.AddChild(rarityEditor);
                rarityEditor.Setup(item.Rarity);

                TagsEditor tagsEditor = (TagsEditor)TagsEditorScene.Instantiate();
                Column2.AddChild(tagsEditor);
                tagsEditor.Setup(item.Tags);
            }
            
            switch (craftable)
            {
                case Building building:
                    BuildingTypeEditor buildingTypeEditor = (BuildingTypeEditor)BuildingTypeScene.Instantiate();
                    Column1.AddChild(buildingTypeEditor);
                    buildingTypeEditor.Setup(building.BuildingType);

                    SceneUidEditor sceneUidEditor = (SceneUidEditor)SceneUidScene.Instantiate();
                    Column2.AddChild(sceneUidEditor);
                    sceneUidEditor.Setup(building.SceneUid);
                    break;
                    
                case CraftingMaterial craftingMaterial:
                    ReusableEditor materialReusableEditor = (ReusableEditor)ReusableEditorScene.Instantiate();
                    Column1.AddChild(materialReusableEditor);
                    materialReusableEditor.Setup(craftingMaterial.Reusable);
                    break;
                    
                case Equipment equipment:
                    EquipmentTypeEditor equipmentTypeEditor = (EquipmentTypeEditor)EquipmentTypeScene.Instantiate();
                    Column1.AddChild(equipmentTypeEditor);
                    equipmentTypeEditor.Setup(equipment.EquipmentType);

                    EquipmentBonusesEditor equipmentBonusesEditor = (EquipmentBonusesEditor)EquipmentBonusesScene.Instantiate();
                    Column2.AddChild(equipmentBonusesEditor);
                    equipmentBonusesEditor.Setup(equipment.EquipmentBonuses);

                    StatsToEquipEditor statsToEquipEditor = (StatsToEquipEditor)StatsToEquipScene.Instantiate();
                    Column1.AddChild(statsToEquipEditor);
                    statsToEquipEditor.Setup(equipment.StatsNeededToEquip);
                    break;
                    
                case UsableItem usableItem:
                    ReusableEditor usableReusableEditor = (ReusableEditor)ReusableEditorScene.Instantiate();
                    Column1.AddChild(usableReusableEditor);
                    usableReusableEditor.Setup(usableItem.Reusable);

                    EffectsEditor effectsEditor = (EffectsEditor)EffectsScene.Instantiate();
                    Column2.AddChild(effectsEditor);
                    effectsEditor.Setup(usableItem.Effects);
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
            SetCraftableProperties();

            if (Craftable.ResourcePath == "")
            {
                if (Craftable.Name == "")
                {
                    AcceptDialog.DialogText = "Craftable Name can't be empty.";
                    AcceptDialog.Show();
                    GD.PushError($"New {Craftable.GetType()} name can't be empty");
                    return;
                }
                switch (Craftable)
                {
                    case CraftingMaterial:
                        string materialPath = $"res://craftables/items/crafting_materials/{Craftable.Name.ToSnakeCase()}.tres";
                        bool craftingMaterialSaved = TrySaveCraftable(materialPath, Databases.Craftables.CraftingMaterials);
                        if (!craftingMaterialSaved)
                        {
                            return;
                        }
                        break;
                    case Equipment:
                        string equipmentPath = $"res://craftables/items/equipment/{Craftable.Name.ToSnakeCase()}.tres";
                        bool equipmentSaved = TrySaveCraftable(equipmentPath, Databases.Craftables.Equipments);
                        if (!equipmentSaved)
                        {
                            return;
                        }
                        break;
                    case UsableItem:
                        string usableItemPath = $"res://craftables/items/usable_items/{Craftable.Name.ToSnakeCase()}.tres";
                        bool usableItemSaved = TrySaveCraftable(usableItemPath, Databases.Craftables.UsableItems);
                        if (!usableItemSaved)
                        {
                            return;
                        }
                        break;
                    case Building:
                        string buildingPath = $"res://craftables/buildings/{Craftable.Name.ToSnakeCase()}.tres";
                        bool buildingSaved = TrySaveCraftable(buildingPath, Databases.Craftables.Buildings);
                        if (!buildingSaved)
                        {
                            return;
                        }
                        break;
                }
            }
            else
            {
                Error error = ResourceSaver.Save(Craftable);
                if (error != Error.Ok)
                {
                    GD.PushError($"Craftable {Craftable.Name} save failed. {error}");
                    return;
                }
            }

            OnSaveCraftable?.Invoke(Craftable.GetUid());
            Hide();
        }

        private void SetCraftableProperties()
        {
            Craftable.Set(Craftable.PropertyName.Name, NameEdit.Text);
            Craftable.Set(Craftable.PropertyName.Description, DescriptionEdit.Text);
            Craftable.Set(Craftable.PropertyName.Icon, IconButton.Icon);

            foreach (Node node in this.GetChildrenRecursive())
            {
                if (node is IPropertyEditor propertyEditor)
                {
                    propertyEditor.SetProperty(Craftable);
                }
            }
        }

        private bool TrySaveCraftable<[MustBeVariant] T>(
            string craftablePath,
            Godot.Collections.Dictionary<long, T> database)
            where T : Craftable
        {
            if (ResourceLoader.Exists(craftablePath))
            {
                AcceptDialog.DialogText = "Craftable Name already taken.";
                AcceptDialog.Show();
                GD.PushError($"Resource already exists at {craftablePath}");
                return false;
            }
            Error error = ResourceSaver.Save(Craftable, craftablePath);
            if (error == Error.Ok)
            {
                Craftable = ResourceLoader.Load<T>(craftablePath);
                database[Craftable.GetUid()] = (T)Craftable;
            }
            else
            {
                GD.PushError($"Craftable {Craftable.Name} save failed at {craftablePath}. Error: {error}");
                return false;
            }

            return true;
        }
    }
}
#endif
