using Godot;

namespace Lastdew
{
	public partial class MugshotCamera : Node3D
	{
		public Image TakeMugshot()
		{
			Camera3D camera = GetNode<Camera3D>("%Camera3D");
			camera.Current = true;
			return GetViewport().GetTexture().GetImage();
		}
	}
}
