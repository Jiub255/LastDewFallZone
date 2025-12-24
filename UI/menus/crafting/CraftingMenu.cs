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
		private TeamData TeamData { get; set; }

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

		public override void _ExitTree()
		{
			base._ExitTree();

			TeamData.Inventory.OnInventoryChanged -= Setup;
			ClearGrids();
		}

		public void SubscribeToEvents(InventoryManager inventory)
		{
			inventory.OnInventoryChanged += Setup;
		}
		
		public void Initialize(TeamData teamData)
		{
			TeamData = teamData;
			SelectedDisplay.Initialize(teamData.Inventory);
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
			if (item.RecipeCosts.Count == 0 ||
			    !item.HasRequiredBuildings(TeamData.Inventory.Buildings) ||
			    !item.HasStatsToCraft(TeamData.MaximumStats))
			{
				return;
			}
			CraftableButton button = (CraftableButton)ButtonScene.Instantiate();
			grid.AddChildDeferred(button);
			button.Initialize(item);
			button.OnPressed += SelectedDisplay.SetItem;
			Buttons.Add(button);
		}

		private void ClearGrids()
		{
			foreach (CraftableButton button in Buttons)
			{
				button.OnPressed -= SelectedDisplay.SetItem;
				button.QueueFree();
			}
			Buttons.Clear();
		}
	}
}
