using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Lastdew
{
	public static class SaveSystem
	{
		private const string SAVE_PATH = "user://savegame.save";
		
		public static void Save(
			InventoryManager inventoryManager,
			TeamData teamData,
			List<BuildingData> buildingDatas)
		{
			SaveData saveData = GatherSaveData(inventoryManager, teamData, buildingDatas);
			
			GD.Print("Saving");
			saveData.PrintData();
			
			string jsonString = JsonSerializer.Serialize(saveData, new JsonSerializerOptions() { IncludeFields = true });

			using FileAccess saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);
			saveFile.StoreLine(jsonString);
		}

		public static SaveData Load()
		{
			if (!FileAccess.FileExists(SAVE_PATH))
			{
				GD.PushError($"No file found at {SAVE_PATH}");
				return null;
			}
			
			using FileAccess saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);

			string jsonString = saveFile.GetLine();
			
			try
			{
				// TODO: Problem is here, its getting saved to file just fine.
				SaveData saveData = JsonSerializer.Deserialize<SaveData>(jsonString);
				
				GD.Print("Loaded");
				saveData.PrintData();
				
				return saveData;
			}
			catch (System.Exception ex)
			{
				GD.PushError("Error deserializing json: " + ex.Message);
				return null;
			}
		}

		public static List<BuildingData> ConvertToBuildingDatas(
			List<BuildingSaveData> datas)
		{
			List<BuildingData> result = [];
			foreach (BuildingSaveData data in datas)
			{
				result.Add(new BuildingData(data));
			}
			return result;
		}

		private static SaveData GatherSaveData(
			InventoryManager inventoryManager,
			TeamData teamData,
			List<BuildingData> buildingDatas)
		{
			Dictionary<long, int> inventory = inventoryManager.GatherSaveData();
			List<PcSaveData> pcSaveDatas = teamData.GatherSaveData();
			return new SaveData(inventory, pcSaveDatas, ConvertFromBuildingDatas(buildingDatas));;
		}

		private static List<BuildingSaveData> ConvertFromBuildingDatas(
			List<BuildingData> buildingDatas)
		{
			List<BuildingSaveData> result = [];
			foreach (BuildingData data in buildingDatas)
			{
				result.Add(new BuildingSaveData(data));
			}
			return result;
		}
	}
}
