using Godot;

namespace Lastdew
{
	[GlobalClass, Tool]
	public partial class PcData : Resource
	{
		[Export]
		public string Name { get; set; }
		[Export]
		public Texture2D Icon { get; set; }
		
		[ExportGroup("Starting Stats")]
		[Export]
		public int Attack { get; set; } = 1;
		[Export]
		public int Defense { get; set; } = 1;
		[Export]
		public int Engineering { get; set; } = 1;
		[Export]
		public int Farming { get; set; } = 1;
		[Export]
		public int Medical { get; set; } = 1;
		[Export]
		public int Scavenging { get; set; } = 1;
		
		[ExportGroup("Mesh Types")]
		[Export]
		public MeshType HeadMesh { get; set; } = MeshType.ADVENTURER;
		[Export]
		public MeshType BodyMesh { get; set; } = MeshType.ADVENTURER;
		[Export]
		public MeshType LegsMesh { get; set; } = MeshType.ADVENTURER;
		[Export]
		public MeshType FeetMesh { get; set; } = MeshType.ADVENTURER;

		public void PrintData()
		{
			GD.Print($"Name: {Name}\n");
			GD.Print($"Icon: {Icon}\n");
			GD.Print($"Starting Attack: {Attack}\n");
			GD.Print($"HeadMesh: {HeadMesh}\n");
		}
	}
}
