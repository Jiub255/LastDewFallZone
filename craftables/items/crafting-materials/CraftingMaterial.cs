using Godot;
using System;

namespace Lastdew
{	
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
	
		// TODO: Is this method going to be used? Should this just inherit Craftable?
		// What would clicking on the "item" do? Show all things it could build?
		public override void OnClickItem(PlayerCharacter pc)
		{
			throw new NotImplementedException();
		}
	}
}
