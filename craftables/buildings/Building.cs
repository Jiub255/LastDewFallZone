using Godot;
using System;

namespace Lastdew
{	
	[GlobalClass, Tool]
	public partial class Building : Craftable
	{
		[Export]
		public BuildingType Type { get; set; }
		// TODO: Store UID instead?
		[Export]
		public PackedScene Scene { get; set; }
		
		public Building(){}
		
		public override void OnClickCraftable()
		{
			throw new NotImplementedException();
		}
	}
}
