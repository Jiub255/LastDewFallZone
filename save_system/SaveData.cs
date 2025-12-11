using System.Collections.Generic;
using Godot;
using Godot.NativeInterop;

namespace Lastdew
{
	/// <summary>
	/// Class that stores all the data before/after serialization. All the necessary data from the 
	/// Inventory/TeamData gets pumped into this, then this gets serialized/deserialized. 
	/// </summary>
	// Constructor only takes parameters that correspond to properties in this class, needed for Json serializer?
	public class SaveData(Dictionary<long, int> inventory, List<PcSaveData> pcSaveDatas)
	{		
		public Dictionary<long, int> Inventory { get; init; } = inventory;
		public List<PcSaveData> PcSaveDatas { get; init; } = pcSaveDatas;

		public void PrintData()
		{
			GD.Print("Inventory\n");
			foreach (KeyValuePair<long, int> kvp in Inventory)
			{
				GD.Print($"key: {kvp.Key}, value: {kvp.Value}\n");
			}

			foreach (PcSaveData saveData in PcSaveDatas)
			{
				saveData.PrintData();
			}
		}
	}
}
