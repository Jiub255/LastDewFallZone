public class InventoryManager
{
	private InventoryController<CraftingMaterial> CraftingMaterials { get; }
	private InventoryController<Equipment> Equipment { get; }
	private InventoryController<UsableItem> UsableItems { get; }
	
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
}
