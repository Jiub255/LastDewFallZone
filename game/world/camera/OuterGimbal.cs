using Godot;

namespace Lastdew
{	
	public partial class OuterGimbal : Node3D
	{
		private float RotationSpeedY { get; set; }
		
		public void Initialize(float rotationSpeedY)
		{
			RotationSpeedY = rotationSpeedY;
		}
		
		public void RotateHorizontal(InputEventMouseMotion inputEvent)
		{
			GlobalRotate(Vector3.Up, -inputEvent.Relative.X * RotationSpeedY);
		}
	}
}
