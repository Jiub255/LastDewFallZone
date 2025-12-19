using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class CraftingMenu : Menu
	{
		private GridContainer AllGrid { get; set; }
		private GridContainer EquipmentGrid { get; set; }
		private GridContainer UsablesGrid { get; set; }
		private GridContainer MaterialsGrid { get; set; }
		private SelectedCraftableDisplay SelectedDisplay { get; set; }
		private PackedScene ButtonScene { get; } = GD.Load<PackedScene>(Uids.CRAFTABLE_BUTTON);
		private List<CraftableButton> Buttons { get; } = [];
		private InventoryManager Inventory { get; set; }

		public override void _Ready()
		{
			base._Ready();

			AllGrid = GetNode<GridContainer>("%AllGrid");
			EquipmentGrid = GetNode<GridContainer>("%EquipmentGrid");
			UsablesGrid = GetNode<GridContainer>("%UsablesGrid");
			MaterialsGrid = GetNode<GridContainer>("%MaterialsGrid");
			SelectedDisplay = GetNode<SelectedCraftableDisplay>("%SelectedCraftableDisplay");
		}

		public override void Open()
		{
			base.Open();
			
			Setup();
		}

		public void ConnectSignals(InventoryManager inventory)
		{
			inventory.Connect(
				InventoryManager.SignalName.OnInventoryChanged,
				Callable.From(Setup));
		}
		
		public void Initialize(InventoryManager inventory)
		{
			Inventory = inventory;
			SelectedDisplay.Initialize(inventory);
		}

		public void Setup()
		{
			ClearGrids();
			foreach (Equipment equipment in Databases.Craftables.Equipments.Values)
			{
				AddButtonToGrids(equipment, EquipmentGrid);
			}
			foreach (UsableItem usableItem in Databases.Craftables.UsableItems.Values)
			{
				AddButtonToGrids(usableItem, UsablesGrid);
			}
			foreach (CraftingMaterial material in Databases.Craftables.CraftingMaterials.Values)
			{
				AddButtonToGrids(material, MaterialsGrid);
			}
		}

		private void AddButtonToGrids(Item item, GridContainer grid)
		{
			AddButtonToGrid(item, grid);
			AddButtonToGrid(item, AllGrid);
		}

		private void AddButtonToGrid(Item item, GridContainer grid)
		{
			if (item.RecipeCosts.Count == 0 || !item.HasRequiredBuildings(Inventory.Buildings))
			{
				return;
			}
			CraftableButton button = (CraftableButton)ButtonScene.Instantiate();
			grid.CallDeferred(Node.MethodName.AddChild, button);
			button.Initialize(item);
			button.Connect(
				CraftableButton.SignalName.OnPressed,
				Callable.From<Item>(SelectedDisplay.SetItem));
			Buttons.Add(button);
		}

		private void ClearGrids()
		{
			foreach (CraftableButton button in Buttons)
			{
				button.QueueFree();
			}
			Buttons.Clear();
		}
	}
}
