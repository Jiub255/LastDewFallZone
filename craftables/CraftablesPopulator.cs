using Godot;
using System;

namespace Lastdew
{
	public partial class CraftablesPopulator : Node
	{
		// TODO: Hard coded paths might break on export. Test it out or do something else.
		private const string DIRECTORY = "res://craftables/";
		private const string PATH = "res://craftables/craftables.tres";
		
		public void PopulateDictionaries()
		{
			/*Buildings.Clear();
			CraftingMaterials.Clear();
			Equipments.Clear();
			UsableItems.Clear();
			
			PopulateDictionaries(DIRECTORY);
			
			this.PrintDebug(
				$"Buildings: {Buildings.Count}, " +
				$"Crafting Materials: {CraftingMaterials.Count}, " +
				$"Equipment: {Equipments.Count}, " +
				$"Usable Items: {UsableItems.Count}");
			Error error = ResourceSaver.Save(this, PATH);
			if (error != Error.Ok)
			{
				this.PrintDebug($"Error saving resource: {error}");
			}
			Testprint();
		}

		private void PopulateDictionaries(string directory)
		{
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
					PopulateDictionaries(subFolder);
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
							Buildings[uid] = building;
							break;
						case CraftingMaterial material:
							CraftingMaterials[uid] = material;
							break;
						case Equipment equipment:
							Equipments[uid] = equipment;
							break;
						case UsableItem usableItem:
							UsableItems[uid] = usableItem;
							break;
					}
				}
			}
			dirAccess.ListDirEnd();
		}

		public void Testprint()
		{
			this.PrintDebug($"Craftables count {Count}");
			foreach (KeyValuePair<long, Craftable> kvp in this)
			{
				this.PrintDebug($"Craftable: {kvp}");
			}*/
		}
	}
}
