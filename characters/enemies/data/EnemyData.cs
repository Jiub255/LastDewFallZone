using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class EnemyData : Resource
	{
		[Export]
		public string EnemyType { get; set; }
		
		[ExportGroup("Movement")]
		[Export]
		public int MaxSpeed { get; set; } = 5;
		[Export]
		public int Acceleration { get; set; } = 35;
		[Export]
		public int TurnSpeed { get; set; } = 360;

		[ExportGroup("Combat")]
		[Export]
		public int Attack { get; set; } = 1;
		[Export]
		public float TimeBetweenAttacks { get; set; } = 2.3f;
		[Export]
		public int MaxHealth { get; set; } = 5;
		[Export]
		public int Defense { get; set; } = 0;
		[Export]
		public float SightDistance { get; set; } = 20f;
		[Export]
		public int Experience { get; set; } = 1;
		[Export]
		public Godot.Collections.Dictionary<Item, int> Loot { get; set; }
	}
}
