using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	// Constructor only takes parameters that correspond to properties in this class, needed for Json serializer?
	public class SaveData(Dictionary<long, int> inventory,
		List<PcSaveData> pcSaveDatas,
		List<BuildingSaveData> buildingDatas)
	{		
		public Dictionary<long, int> Inventory { get; init; } = inventory;
		public List<PcSaveData> PcSaveDatas { get; init; } = pcSaveDatas;
		public List<BuildingSaveData> BuildingDatas { get; init; } = buildingDatas;

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

			foreach (BuildingSaveData buildingData in BuildingDatas)
			{
				GD.Print($"UID: {buildingData.BuildingUid}, " +
				         $"Position: ({buildingData.X}, {buildingData.Y}, {buildingData.Z}), " +
				         $"Rotation: {buildingData.Rotation}\n");
			}
		}
	}
}
