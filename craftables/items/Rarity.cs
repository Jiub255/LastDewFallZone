namespace Lastdew
{	
	// TODO: Might do rarity just as an int, then could just display 0-5 as rare, 5-25 as uncommon, 25-100 as common.
	// Or whatever numbers. It'll calculated doing the cumulative probability thing, so low numbers are more rare.
	// Theoretically no upper limit, but probably not go higher than 100 or so. Really depends on how rare the rarest
	// items will be. 
	public enum Rarity
	{
		COMMON,
		UNCOMMON,
		RARE,
		UNIQUE,
	}
}
