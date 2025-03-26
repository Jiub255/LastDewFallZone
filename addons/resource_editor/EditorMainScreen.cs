#if TOOLS
using Godot;
using System.Collections.Generic;

namespace Lastdew
{
    [Tool]
    public partial class EditorMainScreen : PanelContainer
    {
        private VBoxContainer CraftingMaterialsParent { get; set; }
        private VBoxContainer EquipmentParent { get; set; }
        private VBoxContainer UsableItemsParent { get; set; }
        private VBoxContainer BuildingsParent { get; set; }
        private EditResourcePopup EditResourcePopup { get; set; }
        
        private PackedScene CraftingMaterialDisplayScene { get; } = GD.Load<PackedScene>(UIDs.CRAFTING_MATERIAL_DISPLAY);
        private PackedScene EquipmentDisplayScene { get; } = GD.Load<PackedScene>(UIDs.EQUIPMENT_RESOURCE_DISPLAY);
        private PackedScene UsableItemDisplayScene { get; } = GD.Load<PackedScene>(UIDs.USABLE_ITEM_DISPLAY);
        private PackedScene BuildingDisplayScene { get; } = GD.Load<PackedScene>(UIDs.BUILDING_DISPLAY);

        private Craftables Craftables { get; } = GD.Load<Craftables>(UIDs.CRAFTABLES);

        public override void _Ready()
        {
            base._Ready();
            
            CraftingMaterialsParent = GetNode<VBoxContainer>("%CraftingMaterialsParent");
            EquipmentParent = GetNode<VBoxContainer>("%EquipmentParent");
            UsableItemsParent = GetNode<VBoxContainer>("%UsableItemsParent");
            BuildingsParent = GetNode<VBoxContainer>("%BuildingsParent");
            EditResourcePopup = GetNode<EditResourcePopup>("%EditResourcePopup");
            
            Setup();
        }
        
        private void Setup()
        {
            SetupResourceDisplays(Craftables.Materials.Values, CraftingMaterialsParent, CraftingMaterialDisplayScene);
            SetupResourceDisplays(Craftables.Equipment.Values, EquipmentParent, EquipmentDisplayScene);
            SetupResourceDisplays(Craftables.UsableItems.Values, UsableItemsParent, UsableItemDisplayScene);
            SetupResourceDisplays(Craftables.Buildings.Values, BuildingsParent, BuildingDisplayScene);
        }
        
        private void SetupResourceDisplays(IEnumerable<Craftable> craftables, VBoxContainer parent, PackedScene displayScene)
        {
            // TODO: Separate these by subtype first, so you can subscribe to their specific events.
            // OR, Just put one Action<Craftable> in CraftingDisplay, and then send that through OpenPopup().
            // Then have EditResourcePopup deal with the subtypes.
            foreach (Craftable craftable in craftables)
            {
                CraftableDisplay display = (CraftableDisplay)displayScene.Instantiate();
                parent.AddChild(display);
                display.Setup(craftable);
                // TODO: Where to unsubscribe?
                display.OnOpenPopupPressed += OpenPopup;
            }
        }
        
        private void OpenPopup(Craftable craftable)
        {
            EditResourcePopup.Show();
            EditResourcePopup.Setup(craftable);
        }
    }
}
#endif
