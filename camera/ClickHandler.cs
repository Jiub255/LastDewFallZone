using Godot;

namespace Lastdew
{	
	public abstract partial class ClickHandler : RayCast3D
	{
		// TODO: Base this off of zoom distance?
		private static float RayLength => 2000;
		private Viewport Viewport { get; set; }
		private Camera3D Camera { get; set; }
	
		public abstract void PhysicsProcess(double delta);
		public abstract void UnhandledInput(InputEvent @event);
		
		public override void _Ready()
		{
			base._Ready();
	
			Viewport = GetViewport();
			Camera = Viewport.GetCamera3D();
		}

		protected GodotObject RaycastFromMouse()
		{
			Vector3 rayEnd = ToLocal(Camera.ProjectRayNormal(Viewport.GetMousePosition()) * RayLength);
			TargetPosition = rayEnd;
			ForceRaycastUpdate();
			return GetCollider();
		}
	}
}
