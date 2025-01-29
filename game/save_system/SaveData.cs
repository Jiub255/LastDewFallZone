using System.Collections.Generic;

namespace Lastdew
{
	/// <summary>
	/// Class that stores all the data before/after serialization. All the necessary data from the 
	/// Inventory/TeamData gets pumped into this, then this gets serialized/deserialized. 
	/// </summary>
	public class SaveData
	{
		public Dictionary<string, int> Inventory { get; set; } = new();
		
		// JUST FOR TESTING
		private Craftables Craftables { get; init; }
		
		public SaveData()
		{
			Craftables = Godot.ResourceLoader.Load<Craftables>("res://craftables/craftables.tres");
		}
		
		public SaveData(InventoryManager inventoryManager, TeamData teamData)
		{
			Craftables = Godot.ResourceLoader.Load<Craftables>("res://craftables/craftables.tres");
			
			foreach (KeyValuePair<Item, int> item in inventoryManager)
			{
				Inventory[item.Key.Name] = item.Value;
			}
		}
		
		public void Load(InventoryManager inventoryManager, TeamData teamData)
		{
			foreach (KeyValuePair<string, int> kvp in Inventory)
			{
				this.PrintDebug($"Adding {Craftables[kvp.Key]} to inventory");
				inventoryManager.AddItems((Item)Craftables[kvp.Key], kvp.Value);
			}
		}
	}
}
