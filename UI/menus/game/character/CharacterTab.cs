using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class CharacterTab : PanelContainer
	{	
		private InventoryManager InventoryManager { get; set; }
		private GridContainer ItemsGrid { get; set; }
		private SelectedItemPanel SelectedItemPanel { get; set; }
		private PackedScene ItemButtonScene { get; set; } = GD.Load<PackedScene>(Uids.ITEM_BUTTON);
		private CharacterDisplay CharacterDisplay { get; set; }
		private EquipmentDisplay EquipmentDisplay { get; set; }
		private List<ItemButton> Buttons { get; } = [];
	
		public override void _Ready()
		{
			base._Ready();
			
			ItemsGrid = GetNode<GridContainer>("%ItemsGrid");
			SelectedItemPanel = GetNode<SelectedItemPanel>("%SelectedItemPanel");
			CharacterDisplay = GetNode<CharacterDisplay>("%CharacterDisplay");
			EquipmentDisplay = GetNode<EquipmentDisplay>("%EquipmentDisplay");

			SelectedItemPanel.UseEquip.Pressed += CharacterDisplay.SetupDisplay;
			foreach(Button button in EquipmentDisplay.Buttons)
			{
				button.Pressed += CharacterDisplay.SetupDisplay;
			}
		}
	
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			InventoryManager = inventoryManager;
			CharacterDisplay.Initialize(teamData);
			SelectedItemPanel.Initialize(teamData);
			EquipmentDisplay.Initialize(teamData);

			InventoryManager.OnInventoryChanged += PopulateInventoryUi;
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
			
			InventoryManager.OnInventoryChanged -= PopulateInventoryUi;
			SelectedItemPanel.UseEquip.Pressed -= CharacterDisplay.SetupDisplay;
			
			foreach(Button button in EquipmentDisplay.Buttons)
			{
				button.Pressed -= CharacterDisplay.SetupDisplay;
			}
		}
	
		public void RefreshDisplay()
		{
			PopulateInventoryUi();
			CharacterDisplay.SetupDisplay();
		}
	
		private void PopulateInventoryUi()
		{
			for (int i = Buttons.Count - 1; i >= 0; i--)
			{
				ItemButton button = Buttons[i];
				button.OnPressed -= SelectedItemPanel.SetItem;
				Buttons.RemoveAt(i);
				button.QueueFree();
			}
			
			foreach (KeyValuePair<UsableItem, int> item in InventoryManager.UsableItems)
			{
				SetupButton(item.Key, item.Value);
			}
			foreach (KeyValuePair<Equipment, int> equipment in InventoryManager.Equipment)
			{
				SetupButton(equipment.Key, equipment.Value);
			}
			
			if (Buttons.Count > 0)
			{
				SelectedItemPanel.CallDeferred(SelectedItemPanel.MethodName.SetItem, Buttons[0]);
			}
			else
			{
				SelectedItemPanel.ClearItem();
			}
		}
	
		// TODO: Add a color parameter here to tint similar types of items the same color?
		// Or could go by rarity?
		private void SetupButton(Item item, int amount)
		{
			ItemButton button = (ItemButton)ItemButtonScene.Instantiate();
			ItemsGrid.CallDeferred(Node.MethodName.AddChild, button);
			button.CallDeferred(ItemButton.MethodName.Initialize, item, amount);
			button.OnPressed += SelectedItemPanel.SetItem;
			Buttons.Add(button);
		}
	
		private void RemoveButton(ItemButton button)
		{
			button.OnPressed -= SelectedItemPanel.SetItem;
			button.QueueFree();
		}
	}
}
