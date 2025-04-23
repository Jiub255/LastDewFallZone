using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

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
		// TODO: Replace string with long (UID)? Names will change, UIDs don't.
		[Export]
		public Godot.Collections.Dictionary<long, Building> Buildings { get; set; } = [];
		[Export]
		public Godot.Collections.Dictionary<long, CraftingMaterial> Materials { get; set; } = [];
		[Export]
		public Godot.Collections.Dictionary<long, Equipment> Equipment { get; set;} = [];
		[Export]
		public Godot.Collections.Dictionary<long, UsableItem> UsableItems { get; set; } = [];
		
		public int Count => Buildings.Count + Materials.Count + Equipment.Count + UsableItems.Count;
		
		public Craftable this[long uid]
		{
			get
			{
				if (Buildings.TryGetValue(uid, out Building building))
				{
					return building;
				}
				else if (Materials.TryGetValue(uid, out CraftingMaterial material))
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
		
		public void PopulateDictionaries()
		{
			Buildings.Clear();
			Materials.Clear();
			Equipment.Clear();
			UsableItems.Clear();
			
			PopulateDictionaries(DIRECTORY);
			
			this.PrintDebug(
				$"Buildings: {Buildings.Count}, " +
				$"Materials: {Materials.Count}, " +
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
							Materials[uid] = material;
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
			foreach (KeyValuePair<long, CraftingMaterial> kvp in Materials)
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
