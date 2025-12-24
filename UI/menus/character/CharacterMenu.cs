using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class CharacterMenu : Menu
	{
		private TeamData TeamData { get; set; }
		private GridContainer ItemsGrid { get; set; }
		private SelectedItemPanel SelectedItemPanel { get; set; }
		private PackedScene ItemButtonScene { get; set; } = GD.Load<PackedScene>(Uids.ITEM_BUTTON);
		private CharacterDisplay CharacterDisplay { get; set; }
		private EquipmentDisplay EquipmentDisplay { get; set; }
		private List<ItemButton> Buttons { get; } = [];
		private InventoryManager Inventory { get; set; }
	
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

		public override void _ExitTree()
		{
			base._ExitTree();

			Inventory.OnInventoryChanged -= PopulateInventoryUi;
			SelectedItemPanel.UseEquip.Pressed -= CharacterDisplay.Setup;
			foreach(Button button in EquipmentDisplay.Buttons)
			{
				button.Pressed -= CharacterDisplay.Setup;
			}
			ClearButtons();
		}

		public void SubscribeToEvents(TeamData teamData)
		{
			Inventory = teamData.Inventory;
			Inventory.OnInventoryChanged += PopulateInventoryUi;
			CharacterDisplay.SubscribeToEvents(teamData);
			EquipmentDisplay.SubscribeToEvents(teamData);
		}
	
		public void Initialize(TeamData teamData)
		{
			TeamData = teamData;
			
			// mixed setup/init in these initialize methods
			CharacterDisplay.Initialize(teamData);
			SelectedItemPanel.Initialize(teamData);
			EquipmentDisplay.Initialize(teamData);

		}

		public void Setup()
		{
			CharacterDisplay.Setup();
			EquipmentDisplay.Setup();
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
				itemButton.OnPressed += SelectedItemPanel.SetItem;
				itemButton.QueueFree();
			}

			Buttons.Clear();
		}
	
		private void SetupButtons()
		{
			foreach (KeyValuePair<UsableItem, int> item in TeamData.Inventory.UsableItems)
			{
				SetupButton(item.Key, item.Value);
			}

			foreach (KeyValuePair<Equipment, int> equipment in TeamData.Inventory.Equipment)
			{
				SetupButton(equipment.Key, equipment.Value);
			}
		}

		// TODO: Add a color parameter here to tint similar types of items the same color?
		// Or could go by rarity?
		private void SetupButton(Item item, int amount)
		{
			ItemButton button = (ItemButton)ItemButtonScene.Instantiate();
			ItemsGrid.AddChildDeferred(button);
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
	}
}
