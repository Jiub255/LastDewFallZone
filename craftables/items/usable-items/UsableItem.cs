using Godot;
using System;

namespace Lastdew
{	
	[GlobalClass, Tool]
	public partial class UsableItem : Item
	{
		[Export]
		public bool Reusable { get; set; }
		[Export]
		public Effect[] Effects { get; set; }
	
		public UsableItem() : base()
		{
			Reusable = false;
			Effects = [];
		}
	
		public override void OnClickCraftable()
		{
			throw new NotImplementedException();
		}
	
		public override void OnClickItem(PlayerCharacter pc)
		{
			pc.UseItem(this);
		}
	}
}
