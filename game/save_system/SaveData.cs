using System.Collections.Generic;

namespace Lastdew
{
	/// <summary>
	/// Class that stores all the data before/after serialization. All the necessary data from the 
	/// Inventory/TeamData gets pumped into this, then this gets serialized/deserialized. 
	/// </summary>
	public class SaveData
	{
		// JUST FOR TESTING
		// TODO: How to store/access this in the future? Autoload? Pass it from the top down?
		private readonly Craftables _craftables;
		
		public Dictionary<string, int> Inventory { get; set; } = new();
		public List<PcSaveData> PcSaveDatas { get; set; } = new();
		
		/// <summary>
		/// TODO: Change the constructor to only take parameters that correspond to properties in this class.
		/// Also, make craftables a field so the json serializer ignores it (?)
		/// Then, have SaverLoader to what's currently in the constructor here and basically retrieve the data,
		/// then pass that data into the new constructor for SaveData. Hopefully it works. 
		/// </summary>
		/* public SaveData(InventoryManager inventoryManager, TeamData teamData)
		{
			_craftables = Godot.ResourceLoader.Load<Craftables>("res://craftables/craftables.tres");
			
			foreach (KeyValuePair<Item, int> item in inventoryManager)
			{
				Inventory[item.Key.Name] = item.Value;
			}

			teamData.Save(PcSaveDatas);
		} */
		
		public SaveData(Dictionary<string, int> inventory, List<PcSaveData> pcSaveDatas)
		{
			_craftables = Godot.ResourceLoader.Load<Craftables>("res://craftables/craftables.tres");
			Inventory = inventory;
			PcSaveDatas = pcSaveDatas;
		}
		
		/// <summary>
		/// TODO: Put this in SaverLoader (along with Craftables)? Where should it be?
		/// </summary>
		public void Load(InventoryManager inventoryManager)
		{
			foreach (KeyValuePair<string, int> kvp in Inventory)
			{
				this.PrintDebug($"Adding {_craftables[kvp.Key]} to inventory");
				inventoryManager.AddItems((Item)_craftables[kvp.Key], kvp.Value);
			}
		}
	}
}
