using System.Collections.Specialized;
using Godot;

public partial class LootContainer : StaticBody3D
{
	// TODO: Do random loot based off rarity and tags eventually. Just hand filling item arrays for now.
//	[Export]
//	public Godot.Collections.Array<ItemTags> Tags { get; private set; }
	[Export]
	public ItemAmount[] ItemsWithAmounts { get; private set; }
	[Export]
	public float LootDuration { get; private set; }
	public bool Empty { get; private set; }
	public bool BeingLooted { get; private set; }
	public Vector3 LootingPosition
	{
		get => LootingSpot.GlobalPosition;
	}
	private Node3D LootingSpot { get; set; }

	public override void _Ready()
	{
		base._Ready();
		
		LootingSpot = GetNode<Node3D>("%LootingSpot");
	}
}
