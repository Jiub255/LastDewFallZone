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
		
		[ExportGroup("Mesh Types")]
		[Export]
		public MeshType HeadMesh { get; set; } = MeshType.ADVENTURER;
		[Export]
		public MeshType BodyMesh { get; set; } = MeshType.ADVENTURER;
		[Export]
		public MeshType LegsMesh { get; set; } = MeshType.ADVENTURER;
		[Export]
		public MeshType FeetMesh { get; set; } = MeshType.ADVENTURER;
	}
}
