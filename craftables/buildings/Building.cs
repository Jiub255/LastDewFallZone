using Godot;
using System;

namespace Lastdew
{	
	[GlobalClass, Tool]
	public partial class Building : Craftable
	{
		[Export]
		public BuildingType Type { get; private set; }
		[Export]
		public string SceneUid { get; private set; }
		
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
