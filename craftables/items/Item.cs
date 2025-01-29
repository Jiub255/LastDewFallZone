using Godot;
using Godot.Collections;

namespace Lastdew
{	
	[GlobalClass, Tool]
	public abstract partial class Item : Craftable
	{
		[Export]
		public Rarity ItemRarity { get; private set; } = Rarity.COMMON;
		// Only using Godot array so it'll work in editor.                                                                                                                                                                                                            
		[Export]
		public Array<ItemTags> Tags { get; private set; } = new Array<ItemTags> { };
	
		/// <summary>
		/// For when you click on the item in the inventory menu.
		/// </summary>
		public abstract void OnClickItem(PlayerCharacter pc);
	}
}
