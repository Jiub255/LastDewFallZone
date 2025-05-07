using Godot;
using System;

namespace Lastdew
{	
	[GlobalClass, Tool]
	public partial class Building : Craftable
	{
		[Export]
		public BuildingType Type { get; set; }
		// TODO: Should this be a long (for the uid) instead?
		[Export]
		public string SceneUid { get; set; }
		
		public Building() : base()
		{
			Type = BuildingType.CRAFTING;
		}
		
		public override void OnClickCraftable()
		{
			throw new NotImplementedException();
		}
	}
}
