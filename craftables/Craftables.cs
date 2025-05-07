using Godot;
using System.Collections;
using System.Collections.Generic;

// TODO: Is this class even necessary or useful? Could just Load(uid) any resource at this point.
// Does this just act like a preload and/or centralized location for the craftables?
namespace Lastdew
{
	/// <summary>
	/// Tool script that runs through the craftables directory and adds each craftable to the 
	/// appropriate dictionary(string, Craftable subtype)
	/// </summary>
	[GlobalClass, Tool]
	public partial class Craftables : Resource, IEnumerable
	{
		private const string DIRECTORY = "res://craftables/";
		private const string PATH = "res://craftables/craftables.tres";
		
		// TODO: 1. Is Export necessary to save the dicts with the resource?
		// and 2. Can it be done with C# dictionaries? Esp. if export not needed.
		[Export]
		public Godot.Collections.Dictionary<long, Building> Buildings { get; set; }
		[Export]
		public Godot.Collections.Dictionary<long, CraftingMaterial> CraftingMaterials { get; set; }
		[Export]
		public Godot.Collections.Dictionary<long, Equipment> Equipment { get; set;}
		[Export]
		public Godot.Collections.Dictionary<long, UsableItem> UsableItems { get; set; }
		
		public int Count => Buildings.Count + CraftingMaterials.Count + Equipment.Count + UsableItems.Count;
		
		public Craftable this[long uid]
		{
			get
			{
				if (Buildings.TryGetValue(uid, out Building building))
				{
					return building;
				}
				else if (CraftingMaterials.TryGetValue(uid, out CraftingMaterial material))
				{
					return material;
				}
				else if (Equipment.TryGetValue(uid, out Equipment equipment))
				{
					return equipment;
				}
				else if (UsableItems.TryGetValue(uid, out UsableItem item))
				{
					return item;
				}
				return null;
			}
		}
		
		public Craftables() : base()
		{
			Buildings = [];
			CraftingMaterials = [];
			Equipment = [];
			UsableItems = [];
		}
		
		public void PopulateDictionaries()
		{
			Buildings.Clear();
			CraftingMaterials.Clear();
			Equipment.Clear();
			UsableItems.Clear();
			
			PopulateDictionaries(DIRECTORY);
			
			this.PrintDebug(
				$"Buildings: {Buildings.Count}, " +
				$"Crafting Materials: {CraftingMaterials.Count}, " +
				$"Equipment: {Equipment.Count}, " +
				$"Usable Items: {UsableItems.Count}");
			Error error = ResourceSaver.Save(this, PATH);
			if (error != Error.Ok)
			{
				this.PrintDebug($"Error saving resource: {error}");
			}
			TESTPRINT();
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
					if (fileName == "." || fileName == "..")
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
							Equipment[uid] = equipment;
							break;
						case UsableItem usableItem:
							UsableItems[uid] = usableItem;
							break;
						default:
							break;
					}
				}
			}
			dirAccess.ListDirEnd();
		}

		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<long, Building> kvp in Buildings)
			{
				yield return new KeyValuePair<long, Craftable>(kvp.Key, kvp.Value);
			}
			foreach (KeyValuePair<long, CraftingMaterial> kvp in CraftingMaterials)
			{
				yield return new KeyValuePair<long, Craftable>(kvp.Key, kvp.Value);
			}
			foreach (KeyValuePair<long, Equipment> kvp in Equipment)
			{
				yield return new KeyValuePair<long, Craftable>(kvp.Key, kvp.Value);
			}
			foreach (KeyValuePair<long, UsableItem> kvp in UsableItems)
			{
				yield return new KeyValuePair<long, Craftable>(kvp.Key, kvp.Value);
			}
		}
		
		public bool DeleteCraftable(Craftable craftable)
		{
		    switch (craftable)
		    {
		        case CraftingMaterial craftingMaterial:
					CraftingMaterials.Remove(craftingMaterial.GetUid());
					return true;
		        case Equipment equipment:
					Equipment.Remove(equipment.GetUid());
					return true;
		        case UsableItem usableItem:
					UsableItems.Remove(usableItem.GetUid());
					return true;
		        case Building building:
					Buildings.Remove(building.GetUid());
					return true;
		        default:
					return false;
		    }
		}
		
		public void TESTPRINT()
		{
			this.PrintDebug($"Craftables count {this.Count}");
			foreach (KeyValuePair<long, Craftable> kvp in this)
			{
				this.PrintDebug($"Craftable: {kvp}");
			}
		}
	}
}
