using Godot;

namespace Lastdew
{	
	public struct MovementTarget
	{
		public Vector3 TargetPosition { get; private set; }
		public Node3D Target { get; private set; }
		
		public MovementTarget(Vector3 targetPosition, Node3D target = null)
		{
			TargetPosition = targetPosition;
			Target = target;
		}
	}
}
