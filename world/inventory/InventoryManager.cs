using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public class InventoryManager : IEnumerable<KeyValuePair<Item, int>>
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
		
		public Dictionary<long, int> GatherSaveData()
		{
			Dictionary<long, int> inventory = new();
			foreach (KeyValuePair<Item, int> item in this)
			{
				long uid = ResourceLoader.GetResourceUid(item.Key.ResourcePath);
				inventory[uid] = item.Value;
			}
			return inventory;
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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<KeyValuePair<Item, int>> GetEnumerator()
		{
			foreach (KeyValuePair<CraftingMaterial, int> kvp in CraftingMaterials)
			{
				Item item = kvp.Key;
				KeyValuePair<Item, int> itemKvp = new(item, kvp.Value);
				yield return itemKvp;
			}
			foreach (KeyValuePair<Equipment, int> kvp in Equipment)
			{
				Item item = kvp.Key;
				KeyValuePair<Item, int> itemKvp = new(item, kvp.Value);
				yield return itemKvp;
			}
			foreach (KeyValuePair<UsableItem, int> kvp in UsableItems)
			{
				Item item = kvp.Key;
				KeyValuePair<Item, int> itemKvp = new(item, kvp.Value);
				yield return itemKvp;
			}
		}
	}
}
