using Godot;
using System.Collections;
using System.Collections.Generic;

namespace Lastdew
{	
	public class InventoryController<T> : IEnumerable where T : Item
	{
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
		}
		
		/// <returns>true if item was able to be removed</returns>
		public bool RemoveItems(T item, int amount)
		{
			if (Inventory.ContainsKey(item))
			{
				if (Inventory[item] > amount)
				{
					Inventory[item] -= amount;
					return true;
				}
				else if (Inventory[item] == amount)
				{
					Inventory.Remove(item);
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

		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<T, int> item in Inventory)
			{
				yield return item;
			}
			
			//return Inventory.GetEnumerator();
		}

		public int this[T key]
		{
			get => Inventory[key];
		}
	}
}
