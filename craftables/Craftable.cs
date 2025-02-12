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
		private Godot.Collections.Array<CraftingMaterialAmount> _recipeCosts;
		[Export]
		private Godot.Collections.Array<Building> _requiredBuildings;
		[Export]
		private Godot.Collections.Array<StatAmount> _statRequirements;
		[Export]
		private Godot.Collections.Array<CraftingMaterialAmount> _scrapResults;
		
		public Dictionary<string, int> RecipeCosts
		{
			get
			{
				Dictionary<string, int> dict = new();
				if (_recipeCosts == null)
				{
					return dict;
				}
				
				foreach (CraftingMaterialAmount cma in _recipeCosts)
				{
					dict[cma.Material.Name] = cma.Amount;
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
		public Dictionary<string, int> StatRequirements
		{
			get
			{
				Dictionary<string, int> dict = new();
				foreach (StatAmount sa in _statRequirements)
				{
					dict[sa.Type.ToString()] = sa.Amount;
				}
				return dict;
			}
		}
		public Dictionary<string, int> ScrapResults
		{
			get
			{
				Dictionary<string, int> dict = new();
				foreach (CraftingMaterialAmount cma in _scrapResults)
				{
					dict[cma.Material.Name] = cma.Amount;
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
