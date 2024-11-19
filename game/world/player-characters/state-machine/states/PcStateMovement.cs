using System;
using Godot;

public enum TargetTypes
{
	GROUND,
	LOOT,
	//ENEMY,
}

public struct MovementTarget
{
	public TargetTypes TargetType { get; private set; }
	public Vector3 TargetPosition { get; private set; }
	public Node3D Target { get; private set; }
	
	public MovementTarget(TargetTypes targetType, Vector3 targetPosition, Node3D target = null)
	{
		TargetType = targetType;
		TargetPosition = targetPosition;
		Target = target;
	}
}

public class PcStateMovement : PcState
{
	public Action<float> Move;
	
	public MovementTarget MovementTarget { get; private set; }
	
	// TODO: Get this info from pc stats eventually? Or just have everyone move the same?
	private float MaxSpeed { get; set; } = 7f;
	private float Acceleration { get; set; } = 10f;
	/// <summary>
	/// Degrees per second
	/// </summary>
	private float TurnSpeed { get; set; } = 360f;

	public PcStateMovement(PcStateContext context) : base(context)
	{
		context.NavigationAgent.Connect(
			NavigationAgent3D.SignalName.VelocityComputed,
			Callable.From((Vector3 safeVelocity) => SetSafeVelocity(safeVelocity)));
	}

	public override void PhysicsProcessUnselected(float delta)
	{
		Move?.Invoke(delta);
	}

	public override void PhysicsProcessSelected(float delta)
	{
		Move?.Invoke(delta);
	}

	public override void EnterState(object target)
	{
		if (target is MovementTarget movementTarget)
		{
			MovementTarget = movementTarget;
			Context.NavigationAgent.TargetPosition = movementTarget.TargetPosition;
			this.PrintDebug($"Move target position: {Context.NavigationAgent.TargetPosition}");
			switch (movementTarget.TargetType)
			{
				case TargetTypes.GROUND:
					Move = MoveTowardPoint;
					break;
				case TargetTypes.LOOT:
					Move = MoveTowardLoot;
					break;
				/* case TargetTypes.ENEMY:
					OnMove = MoveTowardEnemy;
					break; */
			}
		}
	}
	
	public override void ExitState() {}
	public override void ProcessUnselected(float delta) {}
	public override void ProcessSelected(float delta) {}

	private void Animate()
	{
		float blendAmount = Mathf.Clamp(Context.Speed / MaxSpeed, 0, 1);
		Context.PcAnimationTree.Set(BlendAmountPath, blendAmount);
	}
	
	private void MoveTowardPoint(float delta)
	{
		if (DestinationReached())
		{
			ChangeState(PcStateNames.IDLE);
			return;
		}
		Animate();
		MoveAndRotate(delta);
	}
	
	private void MoveTowardLoot(float delta)
	{
		if (DestinationReached())
		{
			ChangeState(PcStateNames.LOOTING, MovementTarget.Target);
			return;
		}
		Animate();
		MoveAndRotate(delta);
	}
	
	/* private void MoveTowardEnemy(float delta)
	{
		if (DestinationReached())
		{
			// TODO: Change to Combat.
			return;
		}
		throw new NotImplementedException();
	} */
	
	private bool DestinationReached()
	{
		bool navFinished = Context.NavigationAgent.IsNavigationFinished();
		return navFinished;
	}

	private void MoveAndRotate(float delta)
	{
		Vector3 nextPosition = Context.NavigationAgent.GetNextPathPosition();
		Context.RotateToward(nextPosition, TurnSpeed * delta);
		
		Vector3 direction = (nextPosition - Context.GlobalPosition).Normalized();
		Context.Accelerate(direction * MaxSpeed, Acceleration * delta);
	}

	private void SetSafeVelocity(Vector3 safeVelocity)
	{
		Context.Move(safeVelocity);
	}
}
