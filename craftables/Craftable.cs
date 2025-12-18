using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Lastdew
{
	[GlobalClass, Tool]
	public abstract partial class Craftable : Resource
	{
		[Export]
		public string Name { get; private set; } = "";

		[Export]
		public string Description { get; private set; } = "";

		[Export]
		public Texture2D Icon { get; private set; }

		// Have to use Godot collections below for the resources to load correctly.
		// TODO: Is that still necessary? Could c# collections be used instead?
		
		/// <summary>
		/// Key stored as resource UID. Use Craftables resource to get the actual resource.
		/// </summary>
		[Export]
		public Godot.Collections.Dictionary<long, int> RecipeCosts { get; private set; } = [];

		/// <summary>
		/// Stored as resource UID. Use Craftables resource to get the actual resource.
		/// </summary>
		[Export]
		public Array<long> RequiredBuildings { get; private set; } = [];

		/// <summary>
		/// Key stored as resource UID. Use Craftables resource to get the actual resource.
		/// </summary>
		[Export]
		public Godot.Collections.Dictionary<long, int> ScrapResults { get; private set; } = [];

		[Export]
		public Godot.Collections.Dictionary<StatType, int> StatsNeededToCraft { get; private set; } = [];


		public bool HasEnoughMaterialsToBuild(InventoryManager items)
		{
			return RecipeCosts
				.All((kvp) => items[Databases.Craftables.CraftingMaterials[kvp.Key]] >= kvp.Value);
		}

		public bool HasRequiredBuildings(List<Building> buildings)
		{
			return RequiredBuildings.
				Select(uid => Databases.Craftables.Buildings[uid]).
				All(buildings.Contains);
		}
	}
}
