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
		
		public void RemoveItems(T item, int amount)
		{
			if (Inventory.ContainsKey(item))
			{
				if (Inventory[item] > amount)
				{
					Inventory[item] -= amount;
					OnInventoryChanged?.Invoke();
				}
				else if (Inventory[item] == amount)
				{
					Inventory.Remove(item);
					OnInventoryChanged?.Invoke();
				}
				
				GD.PushWarning($"Not enough of {item.Name} to remove {amount} of them.");
			}
			
			GD.PushWarning($"{item.Name} not in inventory.");
		}
		
		public bool HasItems(T item, int amount)
		{
			return Inventory.ContainsKey(item) && Inventory[item] >= amount;
		}
	}
}
