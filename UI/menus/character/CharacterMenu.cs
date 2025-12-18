using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class CharacterMenu : Menu
	{
		private TeamData TeamData { get; set; }
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

			SelectedItemPanel.UseEquip.Pressed += CharacterDisplay.Setup;
			foreach(Button button in EquipmentDisplay.Buttons)
			{
				button.Pressed += CharacterDisplay.Setup;
			}
		}
	
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			TeamData = teamData;
			InventoryManager = inventoryManager;
			
			// mixed setup/init in these initialize methods
			CharacterDisplay.Initialize(teamData);
			SelectedItemPanel.Initialize(teamData);
			EquipmentDisplay.Initialize(teamData);

			InventoryManager.OnInventoryChanged += PopulateInventoryUi;
		}

		public void Setup()
		{
			CharacterDisplay.Setup();
			EquipmentDisplay.Setup();
		}
	
		public override void _ExitTree()
		{
			base._ExitTree();

			UnsubscribeFromEvents();
		}

		public override void Close()
		{
			base.Close();

			if (TeamData.SelectedIndex != null)
			{
				TeamData.MenuSelectedIndex = (int)TeamData.SelectedIndex;
			}
		}

		private void PopulateInventoryUi()
		{
			ClearButtons();
			SetupButtons();
			SetSelectedItem();
		}

		private void ClearButtons()
		{
			foreach (ItemButton itemButton in Buttons)
			{
				RemoveButton(itemButton);
			}

			Buttons.Clear();
		}
	
		private void RemoveButton(ItemButton button)
		{
			button.OnPressed -= SelectedItemPanel.SetItem;
			button.QueueFree();
		}

		private void SetupButtons()
		{
			foreach (KeyValuePair<UsableItem, int> item in InventoryManager.UsableItems)
			{
				SetupButton(item.Key, item.Value);
			}

			foreach (KeyValuePair<Equipment, int> equipment in InventoryManager.Equipment)
			{
				SetupButton(equipment.Key, equipment.Value);
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

		private void SetSelectedItem()
		{
			if (Buttons.Count > 0)
			{
				SelectedItemPanel.CallDeferred(SelectedItemPanel.MethodName.SetItem, Buttons[0]);
			}
			else
			{
				SelectedItemPanel.ClearItem();
			}
		}

		private void UnsubscribeFromEvents()
		{
			foreach (Node child in ItemsGrid.GetChildren())
			{
				if (child is ItemButton button)
				{
					button.OnPressed -= SelectedItemPanel.SetItem;
				}
			}
			
			foreach(Button button in EquipmentDisplay.Buttons)
			{
				button.Pressed -= CharacterDisplay.Setup;
			}

			SelectedItemPanel.UseEquip.Pressed -= CharacterDisplay.Setup;
			if (InventoryManager != null)
			{
				InventoryManager.OnInventoryChanged -= PopulateInventoryUi;
			}
		}
	}
}
