using System.Collections.Generic;
using Godot;

namespace Lastdew
{	
	public partial class CharacterTab : PanelContainer
	{
		private TeamData TeamData { get; set; }
		private InventoryManager InventoryManager { get; set; }
		private GridContainer ItemsGrid { get; set; }
		private SelectedItemPanel SelectedItemPanel { get; set; }
		private PackedScene ItemButtonScene { get; set; } = GD.Load<PackedScene>("res://game/UI/item_button.tscn");
		private CharacterDisplay CharacterDisplay { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			ItemsGrid = GetNode<GridContainer>("%ItemsGrid");
			SelectedItemPanel = GetNode<SelectedItemPanel>("%SelectedItemPanel");
			CharacterDisplay = GetNode<CharacterDisplay>("%CharacterDisplay");
		}
	
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			TeamData = teamData;
			InventoryManager = inventoryManager;
			CharacterDisplay.Initialize(teamData);
		}
	
		public override void _ExitTree()
		{
			base._ExitTree();
			
			foreach (Node child in ItemsGrid.GetChildren())
			{
				if (child is ItemButton button)
				{
					button.OnPressed -= SetupItemDisplay;
				}
			}
		}
	
		public void PopulateInventoryUI()
		{
			foreach (Node child in ItemsGrid.GetChildren())
			{
				if (child is ItemButton itemButton)
				{
					itemButton.OnPressed -= SetupItemDisplay;
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
			button.OnPressed += SetupItemDisplay;
		}
	
		private void RemoveButton(ItemButton button)
		{
			button.OnPressed -= SetupItemDisplay;
			button.QueueFree();
		}
	
		private void SetupItemDisplay(Item item, int amount)
		{
			SelectedItemPanel.SetItem(item, amount);
		}
	}
}
