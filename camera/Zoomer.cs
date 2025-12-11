using Godot;

namespace Lastdew
{	
	public partial class Zoomer : Node3D
	{
		private float ZoomSensitivity { get; set; }
		private float ZoomSpeed { get; set; }
		private float ZoomMinDistance { get; set; }
		private float ZoomMaxDistance { get; set; }
		private float ZoomDefaultDistance { get; set; }
		
		private float ZoomLevel { get; set; }
	
		public void Initialize(
			float zoomSensitivity,
			float zoomSpeed,
			float zoomMinDistance,
			float zoomMaxDistance,
			float zoomDefaultDistance)
		{
			ZoomSensitivity = zoomSensitivity;
			ZoomSpeed = zoomSpeed;
			ZoomMinDistance = zoomMinDistance;
			ZoomMaxDistance = zoomMaxDistance;
			ZoomDefaultDistance = zoomDefaultDistance;
			
			ZoomLevel = ZoomDefaultDistance;
		}
	
		public override void _Process(double delta)
		{
			base._Process(delta);
	
			Vector3 targetPosition = new(Position.X, Position.Y, ZoomLevel);
			Position = Position.MoveToward(targetPosition, ZoomSpeed * (float)delta);
		}

		public void ResetZoom()
		{
			ZoomLevel = ZoomDefaultDistance;
		}
		
		public void ZoomIn()
		{
			ZoomLevel -= ZoomSensitivity;
			ZoomLevel = Mathf.Clamp(ZoomLevel, ZoomMinDistance, ZoomMaxDistance);
		}
		
		public void ZoomOut()
		{
			ZoomLevel += ZoomSensitivity;
			ZoomLevel = Mathf.Clamp(ZoomLevel, ZoomMinDistance, ZoomMaxDistance);
		}
	}
}
