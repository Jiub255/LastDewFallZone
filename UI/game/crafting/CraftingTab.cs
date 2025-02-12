using System;
using Godot;

namespace Lastdew
{
	public partial class CraftingTab : PanelContainer
	{
		private GridContainer AllGrid { get; set; }
		private GridContainer EquipmentGrid { get; set; }
		private GridContainer UsablesGrid { get; set; }
		private GridContainer MaterialsGrid { get; set; }
		private SelectedCraftableDisplay SelectedDisplay { get; set; }
		private Craftables Craftables { get; set; }
		private PackedScene ButtonScene { get; } = GD.Load<PackedScene>("res://UI/game/crafting/craftable_button.tscn");

		public override void _Ready()
		{
			base._Ready();

			AllGrid = GetNode<GridContainer>("%AllGrid");
			EquipmentGrid = GetNode<GridContainer>("%EquipmentGrid");
			UsablesGrid = GetNode<GridContainer>("%UsablesGrid");
			MaterialsGrid = GetNode<GridContainer>("%MaterialsGrid");
			SelectedDisplay = GetNode<SelectedCraftableDisplay>("%SelectedCraftableDisplay");
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			UnsubscribeButtons(AllGrid);
			UnsubscribeButtons(EquipmentGrid);
			UnsubscribeButtons(UsablesGrid);
			UnsubscribeButtons(MaterialsGrid);
		}
		
		public void Initialize(InventoryManager inventory)
		{
			Craftables = ResourceLoader.Load<Craftables>("res://craftables/craftables.tres");
			PopulateUI();
			SelectedDisplay.Initialize(Craftables, inventory);
		}
		
		private void PopulateUI()
		{
			foreach (Equipment equipment in Craftables.Equipment.Values)
			{
				AddButtonToGrids(equipment, EquipmentGrid);
			}
			foreach (UsableItem usableItem in Craftables.UsableItems.Values)
			{
				AddButtonToGrids(usableItem, UsablesGrid);
			}
			foreach (CraftingMaterial material in Craftables.Materials.Values)
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
			if (item.RecipeCosts.Count == 0)
			{
				return;
			}
			CraftableButton button = (CraftableButton)ButtonScene.Instantiate();
			grid.CallDeferred(GridContainer.MethodName.AddChild, button);
			button.Initialize(item);
			button.OnPressed += SelectedDisplay.SetItem;
		}

		private void UnsubscribeButtons(GridContainer grid)
		{
			foreach (Node node in grid.GetChildren())
			{
				if (node is CraftableButton button)
				{
					button.OnPressed -= SelectedDisplay.SetItem;
				}
			}
		}
	}
}
