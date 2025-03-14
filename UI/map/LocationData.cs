using Godot;
	
namespace Lastdew	
{
	[GlobalClass]
	public partial class LocationData : Resource
	{
		[Export]
		public string Name { get; set; }
		[Export]
		public Texture2D Image { get; set; }
		[Export(PropertyHint.MultilineText)]
		public string Description { get; set; }
		// TODO: Store scene or uid?
		[Export]
		public PackedScene Scene { get; set; }
		[Export]
		public string SceneUid { get; set; }
	}
}
