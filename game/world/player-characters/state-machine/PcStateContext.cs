using Godot;

public class PcStateContext
{
	public Area3D TargetDetector { get; }
	public NavigationAgent3D NavigationAgent { get; }
	public PcAnimationTree PcAnimationTree { get; }
	public Vector3 Position
	{
		get => PC.Position;
	}
	public Vector3 GlobalPosition
	{
		get => PC.GlobalPosition;
	}
	public float Speed
	{
		get => PC.Velocity.Length();
	}
	
	private PlayerCharacter PC { get; }
	
	public PcStateContext(PlayerCharacter pc)
	{
		PC = pc;
		TargetDetector = pc.GetNode<Area3D>("%TargetDetector");
		NavigationAgent = pc.GetNode<NavigationAgent3D>("%NavigationAgent3D");
		PcAnimationTree = pc.GetNode<PcAnimationTree>("%AnimationTree");
	}
	
	public void Move(Vector3 velocity)
	{
		PC.Velocity = velocity;
		PC.MoveAndSlide();
	}
	
	public void RotateToward(Vector3 nextPosition, float turnAmount)
	{
		Vector3 lookTarget = new(
			nextPosition.X,
			PC.GlobalPosition.Y,
			nextPosition.Z);
		Vector3 directionToTarget = (lookTarget - PC.GlobalPosition).Normalized();
		Vector3 forward = PC.GlobalTransform.Basis.Z.Normalized();
		
		float angleToTarget = Mathf.RadToDeg(forward.AngleTo(directionToTarget));
		
		// Check to make sure not rotating the long way around.
		// Cross product is perpendicular to forward and directionToTarget, so it points up or down.
		Vector3 cross = forward.Cross(directionToTarget);
		// If it points down (so dot product < 0), then the angle's sign needs to flip. 
		if (cross.Dot(Vector3.Up) < 0)
		{
			angleToTarget = -angleToTarget;
		}

		float rotationAmount = Mathf.Min(Mathf.Abs(angleToTarget), turnAmount);
		PC.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(rotationAmount * Mathf.Sign(angleToTarget)));
	}
	
	public void Accelerate(Vector3 targetVelocity, float accelerationAmount)
	{
		NavigationAgent.Velocity = PC.Velocity.MoveToward(targetVelocity, accelerationAmount);
	}
}
