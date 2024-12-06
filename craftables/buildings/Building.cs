using Godot;
using System;

namespace Lastdew
{	
	public enum BuildingType
	{
		MEDICAL,
		DEFENSIVE,
		CRAFTING,
		QOL,
		FARMING, // Or production?
		STORAGE,
	}
	
	[GlobalClass]
	public partial class Building : Craftable
	{
		[Export]
		public BuildingType Type { get; set; }
		[Export]
		public PackedScene Scene { get; set; }
		
		public override void OnClickCraftable()
		{
			throw new NotImplementedException();
		}
	}
}
