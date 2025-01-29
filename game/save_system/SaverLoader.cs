using Godot;
using System.Text.Json;

namespace Lastdew
{
	/// <summary>
	/// TODO: Come up with a better name for this class than <c>SaverLoader</c>
	/// </summary>
	public class SaverLoader
	{
		private const string SAVE_PATH = "user://savegame.save";
		
		public SaverLoader() {}
		
		public void Save(InventoryManager inventoryManager, TeamData teamData)
		{
			SaveData saveData = new(inventoryManager, teamData);
			
			string jsonString = JsonSerializer.Serialize(saveData);

			using var saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);

			saveFile.StoreLine(jsonString);
		}
		
		public void Load(InventoryManager inventoryManager, TeamData teamData)
		{
			if (!FileAccess.FileExists(SAVE_PATH))
			{
				GD.PushError($"No file found at {SAVE_PATH}");
				return;
			}
			
			using var saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);

			string jsonString = saveFile.GetLine();
			
			try
			{
				SaveData saveData = JsonSerializer.Deserialize<SaveData>(jsonString);
				saveData.Load(inventoryManager, teamData);
			}
			catch (System.Exception ex)
			{
				GD.PushError("Error deserializing json: " + ex.Message);
			}
		}
	}
}
