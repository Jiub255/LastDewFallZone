using Godot;

namespace Lastdew
{	
	public readonly struct MovementTarget(Vector3 targetPosition, Node3D target = null)
    {
        public Vector3 TargetPosition { get; } = targetPosition;
        public Node3D Target { get; } = target;
    }
}
