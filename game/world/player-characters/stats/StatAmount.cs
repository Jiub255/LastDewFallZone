using Godot;

// TODO: Maybe can get rid of global class/resource once items are in database?
// Possibly just use a Dictionary<StatType, int>?
[GlobalClass]
public partial class StatAmount : Resource
{
	[Export]
	public StatType Type { get; set; }
	[Export]
	public int Amount { get; set;}
	
	public StatAmount(StatType type, int amount)
	{
		Type = type;
		Amount = amount;
	}
}
