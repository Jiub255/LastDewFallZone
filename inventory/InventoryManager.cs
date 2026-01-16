using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public class InventoryManager : IEnumerable<KeyValuePair<Item, int>>
	{
		public event Action OnInventoryChanged;
		public event Action OnFoodChanged;
		public event Action OnWaterChanged;
		public event Action OnAmmoChanged;
		public event Action OnOutOfAmmo; 
		public event Action OnNotOutOfAmmoAnymore; 
		
		private int _food;
		private int _water;
		private int _ammo;

		public InventoryController<CraftingMaterial> CraftingMaterials { get; } = new();
		public InventoryController<Equipment> Equipment { get; } = new();
		public InventoryController<UsableItem> UsableItems { get; } = new();
        public Buildings Buildings { get; set; } = new();

        public int Food
        {
	        get => _food;
	        set
	        {
		        _food = value;
				if (_food < 0)
				{
					// TODO: Set some sort of morale decrease here?
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
			        // TODO: Set some sort of morale decrease here?
			        _water = 0;
		        }
		        OnWaterChanged?.Invoke();
	        }
        }

        public int Ammo
        {
	        get => _ammo;
	        set
	        {
		        if (_ammo == value)
		        {
			        return;
		        }
		        
		        if (_ammo <= 0 && value > 0)
		        {
			        OnNotOutOfAmmoAnymore?.Invoke();
		        }

		        if (_ammo > 0 && value <= 0)
		        {
			        OnOutOfAmmo?.Invoke();
		        }
		        
		        _ammo = value;
		        
		        if (_ammo < 0)
		        {
			        GD.PushError("Ammo should never be less than 0");
			        _ammo = 0;
		        }
		        OnAmmoChanged?.Invoke();
	        }
        }


        public void AddItems(Item item, int amount)
		{
			switch (item)
			{
				case CraftingMaterial craftingMaterial:
					CraftingMaterials.AddItems(craftingMaterial, amount);
					OnInventoryChanged?.Invoke();
					break;
				case Equipment equipment:
					Equipment.AddItems(equipment, amount);
					OnInventoryChanged?.Invoke();
					break;
				case UsableItem usableItem:
					UsableItems.AddItems(usableItem, amount);
					OnInventoryChanged?.Invoke();
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
						OnInventoryChanged?.Invoke();
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
						OnInventoryChanged?.Invoke();
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
						OnInventoryChanged?.Invoke();
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
			foreach ((Item item, int amount) in this)
			{
				long uid = ResourceLoader.GetResourceUid(item.ResourcePath);
				inventory[uid] = amount;
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
			foreach ((CraftingMaterial material, int amount) in CraftingMaterials)
			{
				Item item = material;
				KeyValuePair<Item, int> itemKvp = new(item, amount);
				yield return itemKvp;
			}
			foreach ((Equipment equipment, int amount) in Equipment)
			{
				Item item = equipment;
				KeyValuePair<Item, int> itemKvp = new(item, amount);
				yield return itemKvp;
			}
			foreach ((UsableItem usableItem, int amount) in UsableItems)
			{
				Item item = usableItem;
				KeyValuePair<Item, int> itemKvp = new(item, amount);
				yield return itemKvp;
			}
		}
	}
}
