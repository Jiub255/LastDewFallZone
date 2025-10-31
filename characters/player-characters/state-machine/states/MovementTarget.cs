using Godot;

namespace Lastdew
{	
	public readonly struct MovementTarget
    {
        public MovementTarget()
        {
            TargetPosition = Vector3.Zero;
            Target = null;
        }
        
        public MovementTarget(Vector3 targetPosition, Node3D target = null)
        {
            TargetPosition = targetPosition;
            Target = target;
        }

        public Vector3 TargetPosition { get; }
        public Node3D Target { get; }
    }
}
