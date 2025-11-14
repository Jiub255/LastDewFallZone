using Godot;
	
namespace Lastdew	
{
	[GlobalClass, Tool]
	public partial class LocationData : Resource
	{
		private PackedScene _scene;
		
		[Export]
		public string Name { get; set; }
		[Export]
		public Texture2D Image { get; set; }
		[Export(PropertyHint.MultilineText)]
		public string Description { get; set; }
		// TODO: Store scene or uid?
		[Export]
		public PackedScene Scene
		{
			get => _scene;
			set
			{
				SceneUid = value?.GetUid() ?? 0;
				_scene = value;
			}
		}
		//[Export]
		public long SceneUid { get; private set; }
	}
}
