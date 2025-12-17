using Godot;
using System;

namespace Lastdew
{	
	[GlobalClass, Tool]
	public partial class Building : Craftable
	{
		[Export]
		public BuildingType BuildingType { get; private set; } = BuildingType.CRAFTING;

		// TODO: Use long instead, and have a PackedScene export variable that sets the long Uid
		// in its setter? Like LocationData resource.
		[Export]
		public string SceneUid { get; private set; }

		public override void OnClickCraftable()
		{
			throw new NotImplementedException();
		}
	}
}
