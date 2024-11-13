using Godot;

// Only using this class to fill Loot containers in the inspector. 
// Can probably switch to using Dictionary<Item, int> once item database is implemented,
// along with auto filling loot containers based on item tags and rarity.
[GlobalClass]
public partial class ItemAmount : Resource
{
	[Export]
	public Item Item { get; set; }
	[Export]
	public int Amount { get; set;}
	
	public ItemAmount(Item item, int amount)
	{
		Item = item;
		Amount = amount;
	}
}
