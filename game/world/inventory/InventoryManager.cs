using System;
using System.Collections;
using System.Collections.Generic;

namespace Lastdew
{
	public class InventoryManager : IEnumerable
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

		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<CraftingMaterial, int> kvp in CraftingMaterials)
			{
				Item item = (Item)kvp.Key;
				KeyValuePair<Item, int> itemKvp = new(item, kvp.Value);
				yield return itemKvp;
			}
			foreach (KeyValuePair<Equipment, int> kvp in Equipment)
			{
				Item item = (Item)kvp.Key;
				KeyValuePair<Item, int> itemKvp = new(item, kvp.Value);
				yield return itemKvp;
			}
			foreach (KeyValuePair<UsableItem, int> kvp in UsableItems)
			{
				Item item = (Item)kvp.Key;
				KeyValuePair<Item, int> itemKvp = new(item, kvp.Value);
				yield return itemKvp;
			}
			/* yield return CraftingMaterials.GetEnumerator();
			yield return Equipment.GetEnumerator();
			yield return UsableItems.GetEnumerator(); */
		}

		public int this[Item item]
		{
			get
			{
				if (item is CraftingMaterial material)
				{
					return CraftingMaterials[material];
				}
				else if (item is Equipment equipment)
				{
					return Equipment[equipment];
				}
				else if (item is UsableItem usableItem)
				{
					return UsableItems[usableItem];
				}
				return 0;
			}
		}
		
		public Dictionary<string, int> GetSaveData()
		{
			Dictionary<string, int> inventory = new();
			foreach (KeyValuePair<Item, int> item in this)
			{
				inventory[item.Key.Name] = item.Value;
			}
			return inventory;
		}
	}
}
