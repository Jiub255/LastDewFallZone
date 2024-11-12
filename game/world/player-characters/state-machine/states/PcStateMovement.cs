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
	
	public PcStateMovement(PcStateContext context) : base(context)
	{
		context.NavigationAgent.Connect(
			NavigationAgent3D.SignalName.VelocityComputed,
			Callable.From((Vector3 safeVelocity) => SetSafeVelocity(safeVelocity)));
	}

	public override void EnterState() {}

	public override void ExitState() {}

	public override void ProcessUnselected(float delta) {}

	public override void PhysicsProcessUnselected(float delta)
	{
		Animate();
		if (!Context.NavigationAgent.IsNavigationFinished())
		{
			SetTargetPosition();
			MoveAndRotate(delta);
		}
	}

	public override void ProcessSelected(float delta) {}

	public override void PhysicsProcessSelected(float delta)
	{
		Animate();
		if (!Context.NavigationAgent.IsNavigationFinished())
		{
			SetTargetPosition();
			MoveAndRotate(delta);
		}
	}

	public void SetTargetPosition(Vector3 targetPosition)
	{
		if (Target == null)
		{
			Context.NavigationAgent.TargetPosition = targetPosition;
		}
		GD.Print($"Nav Agent Target position: {Context.NavigationAgent.TargetPosition}");
	}

	public void OnBodyEntered(Node3D body)
	{
		if (body == Target && body is LootContainer lootContainer)
		{
			ChangeState(PcStateNames.LOOTING, lootContainer);
		}
	}

	private void Animate()
	{
		float blendAmount = Mathf.Clamp(Context.Speed / MaxSpeed, 0, 1);
		Context.PcAnimationTree.Set(BlendAmountPath, blendAmount); 
	}

	private void SetTargetPosition()
	{
		if (Target == null)
		{
			return;
		}
		
		Vector3 projection = new Vector3(
			Target.Position.X,
			0,
			Target.Position.Z);
		Context.NavigationAgent.TargetPosition = projection;
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
