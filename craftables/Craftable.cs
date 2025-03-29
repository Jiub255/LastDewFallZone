using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Lastdew
{
	[GlobalClass, Tool]
	public abstract partial class Craftable : Resource
	{
		[Export]
		public string Name { get; private set; } = "Enter Unique Name";
		[Export]
		public string Description { get; private set; } = "Enter Description";
		[Export]
		public Texture2D Icon { get; private set; }

		// Have to use Godot arrays below for the resources to load correctly.
		[Export]
		private Godot.Collections.Dictionary<CraftingMaterial, int> _recipeCosts;
		// TODO: Buildings required to build? Or to have it show up in crafting menu?
		[Export]
		private Godot.Collections.Array<Building> _requiredBuildings;
		// TODO: Stats required to build? Or to have it show up in crafting menu?
		[Export]
		private Godot.Collections.Dictionary<CraftingMaterial, int> _scrapResults;

		[Export]
		public Godot.Collections.Dictionary<StatType, int> StatsNeededToCraft { get; private set; } = [];
		public Dictionary<string, int> RecipeCosts
		{
			get
			{
				Dictionary<string, int> dict = new();
				if (_recipeCosts == null)
				{
					return dict;
				}
				
				foreach (KeyValuePair<CraftingMaterial, int> kvp in _recipeCosts)
				{
					dict[kvp.Key.Name] = kvp.Value;
				}
				return dict;
			}
		}
		public string[] RequiredBuildings
		{
			get
			{
				return _requiredBuildings.Select(x => x.Name).ToArray();
			}
		}
		public Dictionary<string, int> ScrapResults
		{
			get
			{
				Dictionary<string, int> dict = new();
				foreach (KeyValuePair<CraftingMaterial, int> kvp in _scrapResults)
				{
					dict[kvp.Key.Name] = kvp.Value;
				}
				return dict;
			}
		}
	
		/// <summary>
		/// For when you click on the item in the crafting/building menu.
		/// </summary>
		public abstract void OnClickCraftable();
	}
}
