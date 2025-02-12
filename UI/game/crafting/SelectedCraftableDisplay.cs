using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class SelectedCraftableDisplay : PanelContainer
	{
		private Item Item { get; set; }
		private TextureRect Icon { get; set; }
		private Label ItemName { get; set; }
		private Label Description { get; set; }
		private VBoxContainer RecipeCosts { get; set; }
		private Button CraftButton { get; set; }
		private Craftables Craftables { get; set; }
		private InventoryManager Inventory { get; set; }
		private PackedScene RecipeCostScene { get; } = GD.Load<PackedScene>("res://UI/game/crafting/recipe_cost.tscn");

		public override void _Ready()
		{
			base._Ready();
			
			Icon = GetNode<TextureRect>("%Icon");
			ItemName = GetNode<Label>("%Name");
			Description = GetNode<Label>("%Description");
			RecipeCosts = GetNode<VBoxContainer>("%RecipeCosts");
			CraftButton = GetNode<Button>("%CraftButton");

			CraftButton.Pressed += Craft;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			CraftButton.Pressed -= Craft;
		}

		public void Initialize(Craftables craftables, InventoryManager inventory)
		{
			Craftables = craftables;
			Inventory = inventory;
		}

		public void SetItem(Item item)
		{
			Item = item;
			Icon.Texture = item.Icon;
			ItemName.Text = item.Name;
			Description.Text = item.Description;
			
			foreach (Node node in RecipeCosts.GetChildren())
			{
				if (node is RecipeCostUI recipeCost)
				{
					recipeCost.QueueFree();
				}
			}
			CraftButton.Disabled = false;
			foreach (KeyValuePair<string, int> kvp in item.RecipeCosts)
			{
				if (SetupRecipeCostUI(kvp) == false)
				{
					CraftButton.Disabled = true;
				}
			}
		}
		
		public void Craft()
		{
			foreach (KeyValuePair<string, int> kvp in Item.RecipeCosts)
			{
				Inventory.RemoveItems((Item)Craftables[kvp.Key], kvp.Value);
			}
			Inventory.AddItem(Item);
			SetItem(Item);
		}

		/// <returns>true if required amount in inventory</returns>
		private bool SetupRecipeCostUI(KeyValuePair<string, int> kvp)
		{
			RecipeCostUI recipeCost = (RecipeCostUI)RecipeCostScene.Instantiate();
			RecipeCosts.CallDeferred(VBoxContainer.MethodName.AddChild, recipeCost);
			
			CraftingMaterial material = (CraftingMaterial)Craftables[kvp.Key];
			int amountOwned = Inventory[material];
			int amountRequired = kvp.Value;
			
			recipeCost.Initialize(material, amountOwned, amountRequired);

			return amountOwned >= amountRequired;
		}
	}
}
