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
		private SfxButton CraftButton { get; set; }
		private InventoryManager Inventory { get; set; }
		private PackedScene RecipeCostScene { get; set; } = GD.Load<PackedScene>(Uids.RECIPE_COST_DISPLAY);

		public override void _Ready()
		{
			base._Ready();
			
			Icon = GetNode<TextureRect>("%Icon");
			ItemName = GetNode<Label>("%Name");
			Description = GetNode<Label>("%Description");
			RecipeCosts = GetNode<VBoxContainer>("%RecipeCosts");
			CraftButton = GetNode<SfxButton>("%CraftButton");

			CraftButton.Pressed += Craft;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			CraftButton.Pressed -= Craft;
		}

		public void Initialize(InventoryManager inventory)
		{
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
				if (node is RecipeCostUi recipeCost)
				{
					recipeCost.QueueFree();
				}
			}
			CraftButton.Disabled = false;
			foreach (KeyValuePair<long, int> kvp in item.RecipeCosts)
			{
				if (SetupRecipeCostUi(kvp) == false)
				{
					CraftButton.Disabled = true;
				}
			}
		}

		private void Craft()
		{
			foreach ((long uid, int amount) in Item.RecipeCosts)
			{
				CraftingMaterial material = Databases.Craftables.CraftingMaterials[uid];
				if (material.Reusable)
				{
					continue;
				}
				Inventory.RemoveItems(material, amount);
			}
			Inventory.AddItem(Item);
			SetItem(Item);
		}

		/// <returns>true if required amount in inventory</returns>
		private bool SetupRecipeCostUi(KeyValuePair<long, int> kvp)
		{
			RecipeCostUi recipeCost = (RecipeCostUi)RecipeCostScene.Instantiate();
			RecipeCosts.CallDeferred(Node.MethodName.AddChild, recipeCost);
			
			CraftingMaterial material = (CraftingMaterial)Databases.Craftables[kvp.Key];
			int amountOwned = Inventory[material];
			int amountRequired = kvp.Value;
			
			recipeCost.Initialize(material, amountOwned, amountRequired);

			return amountOwned >= amountRequired;
		}
	}
}
