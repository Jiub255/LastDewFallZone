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

		[Export]
		private CraftingMaterialAmount[] _recipeCosts;
		[Export]
		private Building[] _requiredBuildings;
		[Export]
		private StatAmount[] _statRequirements;
		[Export]
		private CraftingMaterialAmount[] _scrapResults;
		
		public Dictionary<string, int> RecipeCosts
		{
			get
			{
				Dictionary<string, int> dict = new();
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
