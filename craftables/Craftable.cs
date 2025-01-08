using Godot;

namespace Lastdew
{	
	public abstract partial class Craftable : Resource
	{
		[Export]
		public string Name { get; private set; } = "Enter Unique Name";
		[Export]
		public string Description { get; private set; } = "Enter Description";
		[Export]
		public Texture2D Icon { get; private set; }
	
		// TODO: How to store recipe data? 
		// Want to eventually have a separate database, then just store arrays of ids here,
		// so no circular references and extra data being stored.
	/* 	[Export]
		public (CraftingMaterial, int)[] RecipeCosts { get; private set; }
		[Export]
		public Building[] RequiredBuildings { get; private set; }
		[Export]
		public (StatType, int)[] StatRequirements { get; private set; }
		[Export]
		public (CraftingMaterial, int)[] ScrapResults { get; private set; } */
	
		/// <summary>
		/// For when you click on the item in the crafting/building menu.
		/// </summary>
		public abstract void OnClickCraftable();
	}
}
