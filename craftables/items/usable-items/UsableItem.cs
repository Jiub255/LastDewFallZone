using Godot;
using System;

[GlobalClass]
public partial class UsableItem : Item
{
	[Export]
	public bool Reusable { get; set; }
	[Export]
	public Effect[] Effects { get; set; }
	
	public override void OnClickCraftable()
	{
		throw new NotImplementedException();
	}

	public override void OnClickItem()
	{
		throw new NotImplementedException();
	}
}