
using System;

namespace Lastdew
{	public class InventoryManager
	{
		public event Action OnInventoryChanged;
		
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
				OnInventoryChanged?.Invoke();
			}
			else if (item is Equipment equipment)
			{
				Equipment.AddItems(equipment, amount);
				OnInventoryChanged?.Invoke();
			}
			else if (item is UsableItem usableItem)
			{
				UsableItems.AddItems(usableItem, amount);
				OnInventoryChanged?.Invoke();
			}
		}
		
		public void AddItem(Item item)
		{
			AddItems(item, 1);
		}
		
		public void RemoveItems(Item item, int amount)
		{
			if (item is CraftingMaterial craftingMaterial)
			{
				if (CraftingMaterials.RemoveItems(craftingMaterial, amount))
				{
					OnInventoryChanged?.Invoke();
				}
			}
			else if (item is Equipment equipment)
			{
				if (Equipment.RemoveItems(equipment, amount))
				{
					OnInventoryChanged?.Invoke();
				}
			}
			else if (item is UsableItem usableItem)
			{
				if (UsableItems.RemoveItems(usableItem, amount))
				{
					OnInventoryChanged?.Invoke();
				}
			}
		}
		
		public void RemoveItem(Item item)
		{
			RemoveItems(item, 1);
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
		
		public bool HasItem(Item item)
		{
			return HasItems(item, 1);
		}
	}
}
