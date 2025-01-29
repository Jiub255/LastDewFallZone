using Godot;
using System.Collections;
using System.Collections.Generic;

namespace Lastdew
{
	[GlobalClass, Tool]
	/// <summary>
	/// Tool script that runs through the craftables directory and adds each craftable to the 
	/// appropriate dictionary<string (name), [Craftable subtype]>
	/// </summary>
	public partial class Craftables : Resource, IEnumerable
	{
		private const string PATH = "res://craftables/";
		
		[Export]
		public Godot.Collections.Dictionary<string, Building> Buildings { get; set; } = new();
		[Export]
		public Godot.Collections.Dictionary<string, CraftingMaterial> Materials { get; set; } = new();
		[Export]
		public Godot.Collections.Dictionary<string, Equipment> Equipment { get; set;} = new();
		[Export]
		public Godot.Collections.Dictionary<string, UsableItem> UsableItems { get; set; } = new();
		
		public int Count => Buildings.Count + Materials.Count + Equipment.Count + UsableItems.Count;
		
		public Craftable this[string name]
		{
			get
			{
				if (Buildings.ContainsKey(name))
				{
					return Buildings[name];
				}
				else if (Materials.ContainsKey(name))
				{
					return Materials[name];
				}
				else if (Equipment.ContainsKey(name))
				{
					return Equipment[name];
				}
				else if (UsableItems.ContainsKey(name))
				{
					return UsableItems[name];
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
			
			PopulateDictionaries(PATH);
			
			this.PrintDebug(
				$"Buildings: {Buildings.Count}, " +
				$"Materials: {Materials.Count}, " +
				$"Equipment: {Equipment.Count}, " +
				$"Usable Items: {UsableItems.Count}");
			Error error = ResourceSaver.Save(this, "res://craftables/craftables.tres");
			if (error != Error.Ok)
			{
				this.PrintDebug($"Error saving resource: {error}");
			}
			TESTPRINT();
		}
		
		private void PopulateDictionaries(string path)
		{
			DirAccess dirAccess = DirAccess.Open(path);
			if (dirAccess == null)
			{
				GD.PushError($"Failed to open directory: {path}");
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
					string subFolder = path + "/" + fileName;
					PopulateDictionaries(subFolder);
				}
				else
				{
					string filePath = path + "/" + fileName;
					Resource resource = GD.Load<Resource>(filePath);
					if (resource != null)
					{
						switch (resource)
						{
							case Building building:
								Buildings[building.Name] = building;
								break;
							case CraftingMaterial material:
								Materials[material.Name] = material;
								break;
							case Equipment equipment:
								Equipment[equipment.Name] = equipment;
								break;
							case UsableItem usableItem:
								UsableItems[usableItem.Name] = usableItem;
								break;
							default:
								break;
						} 
					}
				}
			}
			dirAccess.ListDirEnd();
		}

		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<string, Building> kvp in Buildings)
			{
				yield return new KeyValuePair<string, Craftable>(kvp.Key, (Craftable)kvp.Value);
			}
			foreach (KeyValuePair<string, CraftingMaterial> kvp in Materials)
			{
				yield return new KeyValuePair<string, Craftable>(kvp.Key, (Craftable)kvp.Value);
			}
			foreach (KeyValuePair<string, Equipment> kvp in Equipment)
			{
				yield return new KeyValuePair<string, Craftable>(kvp.Key, (Craftable)kvp.Value);
			}
			foreach (KeyValuePair<string, UsableItem> kvp in UsableItems)
			{
				yield return new KeyValuePair<string, Craftable>(kvp.Key, (Craftable)kvp.Value);
			}
		}
		
		public void TESTPRINT()
		{
			this.PrintDebug($"Craftables count {this.Count}");
			foreach (KeyValuePair<string, Craftable> kvp in this)
			{
				this.PrintDebug($"Craftable: {kvp}");
			}
		}
	}
}
