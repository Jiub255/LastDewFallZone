#if TOOLS
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class EditorMainScreen : PanelContainer
    {
        private TabCraftingMaterials CraftingMaterialsTab { get; set; }
        private TabEquipment EquipmentTab { get; set; }
        private TabUsableItems UsableItemsTab { get; set; }
        private TabBuildings BuildingsTab { get; set; }
        private ResourceEditorPopup ResourceEditorPopup { get; set; }
        private List<TabCraftable> Tabs { get; } = [];

        public override void _Ready()
        {
            base._Ready();

            CraftingMaterialsTab = GetNode<TabCraftingMaterials>("%Crafting Materials");
            EquipmentTab = GetNode<TabEquipment>("%Equipment");
            UsableItemsTab = GetNode<TabUsableItems>("%Usable Items");
            BuildingsTab = GetNode<TabBuildings>("%Buildings");

            Tabs.Add(CraftingMaterialsTab);
            Tabs.Add(EquipmentTab);
            Tabs.Add(UsableItemsTab);
            Tabs.Add(BuildingsTab);
            
            ResourceEditorPopup = GetNode<ResourceEditorPopup>("%ResourceEditorPopup");

            ResourceEditorPopup.OnSaveCraftable += UpdateDisplay;
            foreach (TabCraftable tab in Tabs)
            {
                tab.OnOpenPopupPressed += OpenPopup;
            }
            
            Setup();
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            ResourceEditorPopup.OnSaveCraftable -= UpdateDisplay;
            foreach (TabCraftable tab in Tabs)
            {
                tab.OnOpenPopupPressed -= OpenPopup;
            }
        }
        
        private void Setup()
        {
            foreach (TabCraftable tab in Tabs)
            {
                tab.SetupResourceDisplays();
            }
        }
        
        private void OpenPopup(Craftable craftable)
        {
            ResourceEditorPopup.Show();
            ResourceEditorPopup.Setup(craftable);
        }
        
        private void UpdateDisplay(long uid)
        {
            foreach (TabCraftable tab in Tabs)
            {
                tab.UpdateDisplay(uid);
            }
        }
    }
}
#endif
