using Godot;
using System;
using System.Collections.Generic;

namespace Lastdew
{	
	public class InventoryController<T> where T : Item
	{
		public event Action OnInventoryChanged;
		
		public Dictionary<T, int> Inventory { get; } = new Dictionary<T, int>();
		
		public InventoryController()
		{
			Inventory = new Dictionary<T, int>();
		}
		
		public void AddItems(T item, int amount)
		{
			if (Inventory.ContainsKey(item))
			{
				Inventory[item] += amount;
			}
			else
			{
				Inventory[item] = amount;
			}
	
			OnInventoryChanged?.Invoke();
		}
		
		public bool RemoveItems(T item, int amount)
		{
			if (Inventory.ContainsKey(item))
			{
				if (Inventory[item] > amount)
				{
					Inventory[item] -= amount;
					OnInventoryChanged?.Invoke();
					return true;
				}
				else if (Inventory[item] == amount)
				{
					Inventory.Remove(item);
					OnInventoryChanged?.Invoke();
					return true;
				}
				
				GD.PushWarning($"Not enough of {item.Name} to remove {amount} of them.");
				return false;
			}
			
			GD.PushWarning($"{item.Name} not in inventory.");
			return false;
		}
		
		public bool HasItems(T item, int amount)
		{
			return Inventory.ContainsKey(item) && Inventory[item] >= amount;
		}
	}
}
