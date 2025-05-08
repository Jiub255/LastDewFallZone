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
        private PackedScene StatsToCraftScene { get; } = GD.Load<PackedScene>(UIDs.STATS_TO_CRAFT_EDITOR);
        private PackedScene RecipeCostsScene { get; } = GD.Load<PackedScene>(UIDs.RECIPE_COSTS_EDITOR);
        private PackedScene RequiredBuildingsScene { get; } = GD.Load<PackedScene>(UIDs.REQUIRED_BUILDINGS_EDITOR);
        private PackedScene ScrapResultsScene { get; } = GD.Load<PackedScene>(UIDs.SCRAP_RESULTS_EDITOR);
        private PackedScene RarityEditorScene { get; } = GD.Load<PackedScene>(UIDs.RARITY_EDITOR);
        private PackedScene TagsEditorScene { get; } = GD.Load<PackedScene>(UIDs.TAGS_EDITOR);
        private PackedScene ReusableEditorScene { get; } = GD.Load<PackedScene>(UIDs.REUSABLE_EDITOR);
        private PackedScene EquipmentBonusesScene { get; } = GD.Load<PackedScene>(UIDs.EQUIPMENT_BONUSES_EDITOR);
        private PackedScene EquipmentTypeScene { get; } = GD.Load<PackedScene>(UIDs.EQUIPMENT_TYPE_EDITOR);
        private PackedScene StatsToEquipScene { get; } = GD.Load<PackedScene>(UIDs.STATS_TO_EQUIP_EDITOR);
        private PackedScene BuildingTypeScene { get; } = GD.Load<PackedScene>(UIDs.BUILDING_TYPE_EDITOR);
        private PackedScene SceneUIDScene { get; } = GD.Load<PackedScene>(UIDs.SCENE_UID_EDITOR);
        private PackedScene EffectsScene { get; } = GD.Load<PackedScene>(UIDs.EFFECTS_EDITOR);


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
                rarityEditor.Setup(item.ItemRarity);

                TagsEditor tagsEditor = (TagsEditor)TagsEditorScene.Instantiate();
                Column2.AddChild(tagsEditor);
                tagsEditor.Setup(item.Tags);
            }
            
            switch (craftable)
            {
                case Building building:
                    BuildingTypeEditor buildingTypeEditor = (BuildingTypeEditor)BuildingTypeScene.Instantiate();
                    Column1.AddChild(buildingTypeEditor);
                    buildingTypeEditor.Setup(building.Type);

                    SceneUidEditor sceneUidEditor = (SceneUidEditor)SceneUIDScene.Instantiate();
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
                    equipmentTypeEditor.Setup(equipment.Type);

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
                        string material_path = $"res://craftables/items/crafting-materials/{Craftable.Name.ToSnakeCase()}.tres";
                        bool crafting_material_saved = TrySaveCraftable(material_path, Databases.CRAFTABLES.CraftingMaterials);
                        if (!crafting_material_saved)
                        {
                            return;
                        }
                        break;
                    case Equipment:
                        string equipment_path = $"res://craftables/items/equipment/{Craftable.Name.ToSnakeCase()}.tres";
                        bool equipment_saved = TrySaveCraftable(equipment_path, Databases.CRAFTABLES.Equipments);
                        if (!equipment_saved)
                        {
                            return;
                        }
                        break;
                    case UsableItem:
                        string usable_item_path = $"res://craftables/items/usable-items/{Craftable.Name.ToSnakeCase()}.tres";
                        bool usable_item_saved = TrySaveCraftable(usable_item_path, Databases.CRAFTABLES.UsableItems);
                        if (!usable_item_saved)
                        {
                            return;
                        }
                        break;
                    case Building:
                        string building_path = $"res://craftables/buildings/{Craftable.Name.ToSnakeCase()}.tres";
                        bool building_saved = TrySaveCraftable(building_path, Databases.CRAFTABLES.Buildings);
                        if (!building_saved)
                        {
                            return;
                        }
                        break;
                    default:
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

        private bool TrySaveCraftable<[MustBeVariant] T>(string material_path, Godot.Collections.Dictionary<long, T> database) where T : Craftable
        {
            if (ResourceLoader.Exists(material_path))
            {
                AcceptDialog.DialogText = "Craftable Name already taken.";
                AcceptDialog.Show();
                GD.PushError($"Resource already exists at {material_path}");
                return false;
            }
            Error error = ResourceSaver.Save(Craftable, material_path);
            if (error == Error.Ok)
            {
                Craftable = ResourceLoader.Load<T>(material_path);
                database[Craftable.GetUid()] = (T)Craftable;
            }
            else
            {
                GD.PushError($"Craftable {Craftable.Name} save failed at {material_path}. {error}");
                return false;
            }

            return true;
        }
    }
}
#endif
