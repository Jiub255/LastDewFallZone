#if TOOLS

using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class ResourceDatabaseFiller : EditorPlugin
	{
		private const string CRAFTABLES_DIRECTORY = "res://craftables/";
		private const string CRAFTABLES_PATH = "res://craftables/craftables.tres";
		private const string PC_DATAS_DIRECTORY = "res://characters/player_characters/management/pc_data/";
		private const string PC_DATAS_PATH = "res://characters/player_characters/management/pc_data/all_pc_datas.tres";
		
		private Button CraftablesButton { get; set; }
		private Button PcDatasButton { get; set; }
		
		public override void _EnterTree()
		{
			// Add a custom button to the editor toolbar
			CraftablesButton = new Button
			{
				Text = "Populate Craftables"
			};
			CraftablesButton.Pressed += PopulateCraftables;
			AddControlToContainer(CustomControlContainer.Toolbar, CraftablesButton);
			
			PcDatasButton = new Button
			{
				Text = "Populate PcDatas"
			};
			PcDatasButton.Pressed += PopulateAllPcDatas;
			AddControlToContainer(CustomControlContainer.Toolbar, PcDatasButton);
		}

		public override void _ExitTree()
		{
			CraftablesButton.Pressed -= PopulateCraftables;
			RemoveControlFromContainer(CustomControlContainer.Toolbar, CraftablesButton);
			
			PcDatasButton.Pressed -= PopulateAllPcDatas;
			RemoveControlFromContainer(CustomControlContainer.Toolbar, PcDatasButton);
		}

		private void PopulateCraftables()
		{
			Craftables craftables = Databases.Craftables;
			
			craftables.Buildings.Clear();
			craftables.CraftingMaterials.Clear();
			craftables.Equipments.Clear();
			craftables.UsableItems.Clear();
			
			PopulateCraftables(CRAFTABLES_DIRECTORY);
			
			this.PrintDebug(
				$"Buildings: {craftables.Buildings.Count}, " +
				$"Crafting Materials: {craftables.CraftingMaterials.Count}, " +
				$"Equipment: {craftables.Equipments.Count}, " +
				$"Usable Items: {craftables.UsableItems.Count}");
			Error error = ResourceSaver.Save(craftables, CRAFTABLES_PATH);
			if (error != Error.Ok)
			{
				this.PrintDebug($"Error saving resource: {error}");
			}
			TestPrint();
		}

		private static void PopulateCraftables(string directory)
		{
			Craftables craftables = Databases.Craftables;
			
			DirAccess dirAccess = DirAccess.Open(directory);
			if (dirAccess == null)
			{
				GD.PushError($"Failed to open directory: {directory}");
				return;
			}

			dirAccess.ListDirBegin();
			string fileName;
			while ((fileName = dirAccess.GetNext()) != "")
			{
				if (dirAccess.CurrentIsDir())
				{
					if (fileName is "." or "..")
					{
						continue;
					}

					// Recursively search subfolders
					string subFolder = directory + "/" + fileName;
					PopulateCraftables(subFolder);
				}
				else
				{
					string filePath = directory + "/" + fileName;
					if (!fileName.EndsWith(".tres"))
					{
						continue;
					}
					long uid = ResourceLoader.GetResourceUid(filePath);
					Resource resource = ResourceLoader.Load(filePath, "Craftable");
					switch (resource)
					{
						case Building building:
							craftables.Buildings[uid] = building;
							break;
						case CraftingMaterial material:
							craftables.CraftingMaterials[uid] = material;
							break;
						case Equipment equipment:
							craftables.Equipments[uid] = equipment;
							break;
						case UsableItem usableItem:
							craftables.UsableItems[uid] = usableItem;
							break;
					}
				}
			}
			dirAccess.ListDirEnd();
		}

		private void PopulateAllPcDatas()
		{
			AllPcDatas allPcDatas = Databases.PcDatas;
			
			allPcDatas.PcDatas.Clear();
			
			PopulateAllPcDatas(PC_DATAS_DIRECTORY);
			
			Error error = ResourceSaver.Save(allPcDatas, PC_DATAS_PATH);
			if (error != Error.Ok)
			{
				this.PrintDebug($"Error saving resource: {error}");
			}
			
			this.PrintDebug($"Number of PcDatas: {allPcDatas.PcDatas.Count}");
		}
		
		private static void PopulateAllPcDatas(string directory)
		{
			AllPcDatas allPcDatas = Databases.PcDatas;

			DirAccess dirAccess = DirAccess.Open(directory);
			if (dirAccess == null)
			{
				GD.PushError($"Failed to open directory: {directory}");
				return;
			}

			dirAccess.ListDirBegin();
			string fileName;
			while ((fileName = dirAccess.GetNext()) != "")
			{
				if (dirAccess.CurrentIsDir())
				{
					if (fileName is "." or "..")
					{
						continue;
					}

					// Recursively search subfolders
					string subFolder = directory + "/" + fileName;
					PopulateAllPcDatas(subFolder);
				}
				else
				{
					if (!fileName.EndsWith(".tres"))
					{
						continue;
					}
					string filePath = directory + "/" + fileName;
					Resource resource = GD.Load<Resource>(filePath);
					if (resource is PcData data)
					{
						allPcDatas.PcDatas[data.Name] = data;
					}
				}
			}
			dirAccess.ListDirEnd();
		}
		
		private void TestPrint()
		{
			Craftables craftables = Databases.Craftables;
			this.PrintDebug($"Craftables count {craftables.Count}");
			foreach (KeyValuePair<long, Craftable> kvp in craftables)
			{
				this.PrintDebug($"Craftable: {kvp}");
			}
		}

	}
}
#endif
