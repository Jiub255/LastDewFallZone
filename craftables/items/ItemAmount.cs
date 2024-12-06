using Godot;

namespace Lastdew
{	
	/// <summary>
	/// Only using this class to fill Loot containers in the inspector.
	/// Can probably switch to using Item/int Dictionary once item database is implemented,
	/// along with auto filling loot containers based on item tags and rarity.
	/// </summary>
	[GlobalClass]
	public partial class ItemAmount : Resource
	{
		[Export]
		public Item Item { get; set; }
		[Export]
		public int Amount { get; set;}
		
		public ItemAmount() {}
		
		public ItemAmount(Item item, int amount)
		{
			Item = item;
			Amount = amount;
		}
	}
}
