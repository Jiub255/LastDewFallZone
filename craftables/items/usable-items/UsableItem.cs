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
		public Effect[] Effects { get; set; } = Array.Empty<Effect>();
	
		public UsableItem(){}
	
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
