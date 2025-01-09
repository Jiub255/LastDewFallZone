using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class CharacterTab : PanelContainer
	{
		private InventoryManager InventoryManager { get; set; }
		private GridContainer ItemsGrid { get; set; }
		private SelectedItemPanel SelectedItemPanel { get; set; }
		private PackedScene ItemButtonScene { get; set; } = GD.Load<PackedScene>("res://game/UI/item_button.tscn");
		private CharacterDisplay CharacterDisplay { get; set; }
		private EquipmentDisplay EquipmentDisplay { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			ItemsGrid = GetNode<GridContainer>("%ItemsGrid");
			SelectedItemPanel = GetNode<SelectedItemPanel>("%SelectedItemPanel");
			CharacterDisplay = GetNode<CharacterDisplay>("%CharacterDisplay");
			EquipmentDisplay = GetNode<EquipmentDisplay>("%EquipmentDisplay");
		}
	
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			InventoryManager = inventoryManager;
			CharacterDisplay.Initialize(teamData);
			SelectedItemPanel.Initialize(teamData);
			EquipmentDisplay.Initialize(teamData);

			InventoryManager.OnInventoryChanged += PopulateInventoryUI;
		}
	
		public override void _ExitTree()
		{
			base._ExitTree();
			
			foreach (Node child in ItemsGrid.GetChildren())
			{
				if (child is ItemButton button)
				{
					button.OnPressed -= SelectedItemPanel.SetItem;
				}
			}
			
			InventoryManager.OnInventoryChanged -= PopulateInventoryUI;
		}
	
		public void PopulateInventoryUI()
		{
			foreach (Node child in ItemsGrid.GetChildren())
			{
				if (child is ItemButton itemButton)
				{
					itemButton.OnPressed -= SelectedItemPanel.SetItem;
					itemButton.QueueFree();
				}
			}
			
			foreach (KeyValuePair<UsableItem, int> item in InventoryManager.UsableItems.Inventory)
			{
				SetupButton(item.Key, item.Value);
			}
			foreach (KeyValuePair<Equipment, int> equipment in InventoryManager.Equipment.Inventory)
			{
				SetupButton(equipment.Key, equipment.Value);
			}
		}
	
		// TODO: Add a color parameter here to tint similar types of items the same color?
		// Or could go by rarity?
		private void SetupButton(Item item, int amount)
		{
			ItemButton button = (ItemButton)ItemButtonScene.Instantiate();
			ItemsGrid.CallDeferred(GridContainer.MethodName.AddChild, button);
			button.CallDeferred(ItemButton.MethodName.Initialize, item, amount);
			button.OnPressed += SelectedItemPanel.SetItem;
		}
	
		private void RemoveButton(ItemButton button)
		{
			button.OnPressed -= SelectedItemPanel.SetItem;
			button.QueueFree();
		}
	}
}
