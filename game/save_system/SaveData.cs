using System;
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
		public List<PcSaveData> PcSaveDatas { get; set; } = new();
		
		// Constructor only takes parameters that correspond to properties in this class, needed for Json serializer?
		public SaveData(Dictionary<string, int> inventory, List<PcSaveData> pcSaveDatas)
		{
			Inventory = inventory;
			PcSaveDatas = pcSaveDatas;
		}
		
		public void PrintData()
		{
			foreach (KeyValuePair<string, int> item in Inventory)
			{
				this.PrintDebug($"{item.Key}: {item.Value}");
			}
			foreach (PcSaveData pc in PcSaveDatas)
			{
				this.PrintDebug($"{pc.Name}, Equipment: {pc.Head}, {pc.Weapon}, {pc.Body}, {pc.Feet}, Injury: {pc.Injury}");
			}
		}
	}
}
