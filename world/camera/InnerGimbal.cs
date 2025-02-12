using Godot;

namespace Lastdew
{	
	public partial class InnerGimbal : Node3D
	{
		private float RotationMinX { get; set; }
		private float RotationMaxX { get; set; }
		private float RotationSpeedX { get; set; }
		
		public void Initialize(float rotationMinX, float rotationMaxX, float rotationSpeedX)
		{
			RotationMinX = rotationMinX;
			RotationMaxX = rotationMaxX;
			RotationSpeedX = rotationSpeedX;
		}
		
		public void RotateVertical(InputEventMouseMotion inputEvent)
		{
			RotateObjectLocal(Vector3.Right, -inputEvent.Relative.Y * RotationSpeedX);
			float clampedX = Mathf.Clamp(RotationDegrees.X, -RotationMaxX, -RotationMinX);
			RotationDegrees = new Vector3(clampedX, RotationDegrees.Y, RotationDegrees.Z);
		}
	}
}
