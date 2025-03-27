using Godot;
using System;

namespace Lastdew
{	
	[GlobalClass, Tool]
	public partial class Building : Craftable
	{
		[Export]
		public BuildingType Type { get; set; }
		[Export]
		public string SceneUid { get; set; }
		
		public Building(){}
		
		public override void OnClickCraftable()
		{
			throw new NotImplementedException();
		}
	}
}
