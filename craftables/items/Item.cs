using Godot;
using System;

public abstract partial class Item : Craftable
{
	public enum Rarity
	{
		COMMON = 100,
		UNCOMMON = 50,
		RARE = 10,
		UNIQUE = 1,
	}
	
	// TODO: Might not do this.
	// Used by loot system, pull randomly from pool of items containing any of a set of tags?
	public enum ItemTags
	{
		COMBAT,
		MEDICAL,
		WEAPON,
		ARMOR,
		CRAFTING_MATERIAL,
	}
	
	[Export]
	public Rarity ItemRarity { get; private set; } = Rarity.COMMON;
	// Only using Godot array so it'll work in editor.
	[Export]
	public Godot.Collections.Array<ItemTags> Tags { get; private set; }

	/// <summary>
	/// For when you click on the item in the inventory menu.
	/// </summary>
	public abstract void OnClickItem();
}
