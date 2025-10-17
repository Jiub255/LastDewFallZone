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
		private PackedScene ButtonScene { get; } = GD.Load<PackedScene>(UIDs.CRAFTABLE_BUTTON);

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
			PopulateUI();
			SelectedDisplay.Initialize(Databases.CRAFTABLES, inventory);
		}
		
		private void PopulateUI()
		{
			foreach (Equipment equipment in Databases.CRAFTABLES.Equipments.Values)
			{
				AddButtonToGrids(equipment, EquipmentGrid);
			}
			foreach (UsableItem usableItem in Databases.CRAFTABLES.UsableItems.Values)
			{
				AddButtonToGrids(usableItem, UsablesGrid);
			}
			foreach (CraftingMaterial material in Databases.CRAFTABLES.CraftingMaterials.Values)
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
			grid.CallDeferred(Node.MethodName.AddChild, button);
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
