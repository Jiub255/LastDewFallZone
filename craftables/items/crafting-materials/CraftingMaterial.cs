using Godot;
using System;

[GlobalClass]
public partial class CraftingMaterial : Item
{
	// Instead of a Tool subclass of item, just doing reusable crafting items.
	// Just put the tool in the crafting items slot when making something, and 
	// it just won't get used up. 
	[Export]
	public bool Reusable { get; private set; }

	public override void OnClickCraftable()
	{
		throw new NotImplementedException();
	}

	public override void OnClickItem()
	{
		throw new NotImplementedException();
	}
}
