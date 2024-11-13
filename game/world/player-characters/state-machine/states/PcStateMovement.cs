using Godot;

public class PcStateMovement : PcState
{
	// TODO: Get this info from pc stats eventually? Or just have everyone move the same?
	private float MaxSpeed { get; set; } = 7f;
	private float Acceleration { get; set; } = 10f;
	/// <summary>
	/// Degrees per second
	/// </summary>
	private float TurnSpeed { get; set; } = 360f;
	private string BlendAmountPath { get; } = "parameters/movement_blend_tree/idle_move/blend_amount";

	// TODO: Implement this instead of using Target in PcState. Use this in this state,
	// then have LootContainer in Loot and Enemy in Combat. Only pass the necessary data.
	private enum TargetTypes
	{
		GROUND,
		LOOT,
		ENEMY,
	}
	private struct MovementTarget
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

	public PcStateMovement(PcStateContext context) : base(context)
	{
		context.NavigationAgent.Connect(
			NavigationAgent3D.SignalName.VelocityComputed,
			Callable.From((Vector3 safeVelocity) => SetSafeVelocity(safeVelocity)));
	}

	public override void PhysicsProcessUnselected(float delta)
	{
		HandleMovement(delta);
	}

	public override void PhysicsProcessSelected(float delta)
	{
		HandleMovement(delta);
	}

	public void SetTargetPosition(Vector3 targetPosition)
	{
		if (Target == null)
		{
			Context.NavigationAgent.TargetPosition = targetPosition;
		}
		else if (Target is LootContainer lootContainer)
		{
			Context.NavigationAgent.TargetPosition = lootContainer.LootingPosition;
		}
		this.PrintDebug($"Nav Agent Target position: {Context.NavigationAgent.TargetPosition}");
	}

	public void OnBodyEntered(Node3D body)
	{
		if (body == Target && body is LootContainer lootContainer)
		{
			ChangeState(PcStateNames.LOOTING, lootContainer);
		}
	}

	public override void EnterState() {}
	public override void ExitState() {}
	public override void ProcessUnselected(float delta) {}
	public override void ProcessSelected(float delta) {}

	private void Animate()
	{
		float blendAmount = Mathf.Clamp(Context.Speed / MaxSpeed, 0, 1);
		Context.PcAnimationTree.Set(BlendAmountPath, blendAmount); 
	}

	private void HandleMovement(float delta)
	{
		Animate();
		//this.PrintDebug($"Nav finished: {Context.NavigationAgent.IsNavigationFinished()}");
		if (!Context.NavigationAgent.IsNavigationFinished())
		{
			// TODO: Should this really be set each frame? Only for enemies, who can move.
 			/*if (Target is Enemy)
			{
				SetTargetPosition();
			}*/
			MoveAndRotate(delta);
		}
		else
		{
			//this.PrintDebug($"Target is loot: {Target is LootContainer}");
			this.PrintDebug($"Target type: {Target?.GetType()}");
			if (Target is LootContainer lootContainer)
			{
				// TODO: Change to loot state. How to pass LootContainer to Loot state?
				ChangeState(PcStateNames.LOOTING, lootContainer);
			}
		}
		// TODO: Change to Combat state here.
	}

/* 	private void SetTargetPosition()
	{
		Vector3 projection = new Vector3(
			Target.Position.X,
			0,
			Target.Position.Z);
		Context.NavigationAgent.TargetPosition = projection;
	} */

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
