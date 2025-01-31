using Godot;
using System.Collections.Generic;
using System.Text.Json;

namespace Lastdew
{
	/// <summary>
	/// TODO: Come up with a better name for this class than <c>SaverLoader</c>
	/// </summary>
	public static class SaverLoader
	{
		private const string SAVE_PATH = "user://savegame.save";
		
		public static void Save(InventoryManager inventoryManager, TeamData teamData)
		{
			Dictionary<string, int> inventory = inventoryManager.GetSaveData();
			List<PcSaveData> pcSaveDatas = teamData.GetSaveData();
			SaveData saveData = new(inventory, pcSaveDatas);
			
			string jsonString = JsonSerializer.Serialize(saveData);

			using var saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);

			saveFile.StoreLine(jsonString);
		}
		
		public static List<PcSaveData> Load(InventoryManager inventoryManager)
		{
			if (!FileAccess.FileExists(SAVE_PATH))
			{
				GD.PushError($"No file found at {SAVE_PATH}");
				return null;
			}
			
			using var saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);

			string jsonString = saveFile.GetLine();
			
			try
			{
				SaveData saveData = JsonSerializer.Deserialize<SaveData>(jsonString);
				saveData.Load(inventoryManager);
				return saveData.PcSaveDatas;
			}
			catch (System.Exception ex)
			{
				GD.PushError("Error deserializing json: " + ex.Message);
				return null;
			}
		}
	}
}
