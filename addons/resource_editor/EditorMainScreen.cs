#if TOOLS
using Godot;
using System;
using System.Collections.Generic;

namespace Lastdew
{
    [Tool]
    public partial class EditorMainScreen : PanelContainer
    {
        private TabCraftingMaterials CraftingMaterialsTab { get; set; }
    
        private VBoxContainer CraftingMaterialsParent { get; set; }
        private VBoxContainer EquipmentParent { get; set; }
        private VBoxContainer UsableItemsParent { get; set; }
        private VBoxContainer BuildingsParent { get; set; }
        private ResourceEditorPopup EditResourcePopup { get; set; }
        
        private PackedScene CraftingMaterialDisplayScene { get; } = GD.Load<PackedScene>(UIDs.CRAFTING_MATERIAL_DISPLAY);
        private PackedScene EquipmentDisplayScene { get; } = GD.Load<PackedScene>(UIDs.EQUIPMENT_RESOURCE_DISPLAY);
        private PackedScene UsableItemDisplayScene { get; } = GD.Load<PackedScene>(UIDs.USABLE_ITEM_DISPLAY);
        private PackedScene BuildingDisplayScene { get; } = GD.Load<PackedScene>(UIDs.BUILDING_DISPLAY);

        private Dictionary<long, CraftableDisplay> DisplaysByUid { get; } = [];

        public override void _Ready()
        {
            base._Ready();

            CraftingMaterialsTab = GetNode<TabCraftingMaterials>("%CraftingMaterials");
            
            CraftingMaterialsParent = GetNode<VBoxContainer>("%CraftingMaterialsParent");
            EquipmentParent = GetNode<VBoxContainer>("%EquipmentParent");
            UsableItemsParent = GetNode<VBoxContainer>("%UsableItemsParent");
            BuildingsParent = GetNode<VBoxContainer>("%BuildingsParent");
            EditResourcePopup = GetNode<ResourceEditorPopup>("%EditResourcePopup");

            CraftingMaterialsTab.OnCreateNew += CreateNewCraftable;

            EditResourcePopup.OnSaveCraftable += UpdateDisplay;
            
            Setup();
        }

        private void CreateNewCraftable(Type type)
        {
            if (!typeof(Craftable).IsAssignableFrom(type))
            {
                throw new ArgumentException("Type must inherit from Craftable", nameof(type));
            }
            Craftable craftable = (Craftable)Activator.CreateInstance(type);
            
            // TODO: Finish setting up craftable.
            // TODO: Cast it to its subtype when passed back to its tab?
            // TODO: Add to Craftables database.
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            CraftingMaterialsTab.OnCreateNew -= CreateNewCraftable;

            EditResourcePopup.OnSaveCraftable -= UpdateDisplay;
        }
        
        private void Setup()
        {
            Craftables craftables = Databases.CRAFTABLES;
            SetupResourceDisplays(craftables.Materials.Values, CraftingMaterialsParent, CraftingMaterialDisplayScene);
            SetupResourceDisplays(craftables.Equipment.Values, EquipmentParent, EquipmentDisplayScene);
            SetupResourceDisplays(craftables.UsableItems.Values, UsableItemsParent, UsableItemDisplayScene);
            SetupResourceDisplays(craftables.Buildings.Values, BuildingsParent, BuildingDisplayScene);
        }
        
        private void SetupResourceDisplays(IEnumerable<Craftable> craftables, VBoxContainer parent, PackedScene displayScene)
        {
            foreach (Craftable craftable in craftables)
            {
                CraftableDisplay display = (CraftableDisplay)displayScene.Instantiate();
                parent.AddChild(display);
                display.Setup(craftable);
                // TODO: Where to unsubscribe? On delete resource, and on ExitTree?
                display.OnOpenPopupPressed += OpenPopup;
                DisplaysByUid[craftable.GetUid()] = display;
            }
        }
        
        private void OpenPopup(Craftable craftable)
        {
            EditResourcePopup.Show();
            EditResourcePopup.Setup(craftable);
        }
        
        private void UpdateDisplay(long uid)
        {
            DisplaysByUid[uid].Setup(Databases.CRAFTABLES[uid]);
        }
    }
}
#endif
