using System.Collections.Generic;
using Godot;

namespace Lastdew
{	
	public partial class LootContainer : StaticBody3D
	{
		// TODO: Do random loot based off rarity and tags eventually. Just hand filling item arrays for now.
	//	[Export]
		public Godot.Collections.Array<ItemTags> Tags { get; private set; }
		[Export]
		public Godot.Collections.Dictionary<Item, int> Loot { get; private set; }
		[Export]
		public float LootDuration { get; private set; }
		public bool Empty { get; set; }
		public bool BeingLooted { get; set; }
		public Vector3 LootingPosition
		{
			get => LootingSpot.GlobalPosition;
		}
		private Node3D LootingSpot { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			LootingSpot = GetNode<Node3D>("LootingSpot");
		}
	}
}
