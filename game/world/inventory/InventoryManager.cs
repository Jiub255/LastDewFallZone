
namespace Lastdew
{	public class InventoryManager
	{
		public InventoryController<CraftingMaterial> CraftingMaterials { get; }
		public InventoryController<Equipment> Equipment { get; }
		public InventoryController<UsableItem> UsableItems { get; }
		
		public InventoryManager()
		{
			CraftingMaterials = new InventoryController<CraftingMaterial>();
			Equipment = new InventoryController<Equipment>();
			UsableItems = new InventoryController<UsableItem>();
		}
		
		public void AddItems(Item item, int amount)
		{
			if (item is CraftingMaterial craftingMaterial)
			{
				CraftingMaterials.AddItems(craftingMaterial, amount);
			}
			else if (item is Equipment equipment)
			{
				Equipment.AddItems(equipment, amount);
			}
			else if (item is UsableItem usableItem)
			{
				UsableItems.AddItems(usableItem, amount);
			}
		}
		
		public bool RemoveItems(Item item, int amount)
		{
			if (item is CraftingMaterial craftingMaterial)
			{
				return CraftingMaterials.RemoveItems(craftingMaterial, amount);
			}
			else if (item is Equipment equipment)
			{
				return Equipment.RemoveItems(equipment, amount);
			}
			else if (item is UsableItem usableItem)
			{
				return UsableItems.RemoveItems(usableItem, amount);
			}
			
			return false;
		}
		
		public bool HasItems(Item item, int amount)
		{
			if (item is CraftingMaterial craftingMaterial)
			{
				return CraftingMaterials.HasItems(craftingMaterial, amount);
			}
			else if (item is Equipment equipment)
			{
				return Equipment.HasItems(equipment, amount);
			}
			else if (item is UsableItem usableItem)
			{
				return UsableItems.HasItems(usableItem, amount);
			}
			
			return false;
		}
	}
}
