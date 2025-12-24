using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class InventoryManager : RefCounted, IEnumerable<KeyValuePair<Item, int>>
	{
		[Signal]
		public delegate void OnInventoryChangedEventHandler();
		//public event Action OnInventoryChanged;
		public event Action OnFoodChanged;
		public event Action OnWaterChanged;
		
		private int _food;
		private int _water;

		public InventoryController<CraftingMaterial> CraftingMaterials { get; } = new();
		public InventoryController<Equipment> Equipment { get; } = new();
		public InventoryController<UsableItem> UsableItems { get; } = new();
        public List<BuildingData> Buildings { get; set; } = [];

        public int Food
        {
	        get => _food;
	        set
	        {
		        _food = value;
				if (_food < 0)
				{
					_food = 0;
				}
				OnFoodChanged?.Invoke();
	        }
        }

        public int Water
        {
	        get => _water;
	        set
	        {
		        _water = value;
				if (_water < 0)
				{
					_water = 0;
				}
				OnWaterChanged?.Invoke();
	        }
        }


        public void AddItems(Item item, int amount)
		{
			switch (item)
			{
				case CraftingMaterial craftingMaterial:
					CraftingMaterials.AddItems(craftingMaterial, amount);
					//OnInventoryChanged?.Invoke();
					EmitSignal(SignalName.OnInventoryChanged);
					break;
				case Equipment equipment:
					Equipment.AddItems(equipment, amount);
					//OnInventoryChanged?.Invoke();
					EmitSignal(SignalName.OnInventoryChanged);
					break;
				case UsableItem usableItem:
					UsableItems.AddItems(usableItem, amount);
					//OnInventoryChanged?.Invoke();
					EmitSignal(SignalName.OnInventoryChanged);
					break;
			}
		}
		
		public void AddItem(Item item)
		{
			AddItems(item, 1);
		}
		
		public void RemoveItems(Item item, int amount)
		{
			switch (item)
			{
				case CraftingMaterial craftingMaterial:
				{
					if (CraftingMaterials.RemoveItems(craftingMaterial, amount))
					{
						//OnInventoryChanged?.Invoke();
						EmitSignal(SignalName.OnInventoryChanged);
					}
					else
					{
						GD.PushError($"Not enough in inventory to remove {amount} {craftingMaterial}");
					}
					break;
				}
				case Equipment equipment:
				{
					if (Equipment.RemoveItems(equipment, amount))
					{
						//OnInventoryChanged?.Invoke();
						EmitSignal(SignalName.OnInventoryChanged);
					}
					else
					{
						GD.PushError($"Not enough in inventory to remove {amount} {equipment}");
					}
					break;
				}
				case UsableItem usableItem:
				{
					if (UsableItems.RemoveItems(usableItem, amount))
					{
						//OnInventoryChanged?.Invoke();
						EmitSignal(SignalName.OnInventoryChanged);
					}
					else
					{
						GD.PushError($"Not enough in inventory to remove {amount} {usableItem}");
					}
					break;
				}
			}
		}
		
		public void RemoveItem(Item item)
		{
			RemoveItems(item, 1);
		}
		
		public bool HasItems(Item item, int amount)
		{
			return item switch
			{
				CraftingMaterial craftingMaterial => CraftingMaterials.HasItems(craftingMaterial, amount),
				Equipment equipment => Equipment.HasItems(equipment, amount),
				UsableItem usableItem => UsableItems.HasItems(usableItem, amount),
				_ => false
			};
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
				return item switch
				{
					CraftingMaterial material => CraftingMaterials[material],
					Equipment equipment => Equipment[equipment],
					UsableItem usableItem => UsableItems[usableItem],
					_ => 0
				};
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
