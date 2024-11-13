using Godot;
using System.Collections.Generic;

public enum PcStateNames
{
	IDLE,
	MOVEMENT,
	LOOTING,
	//COMBAT,
}

public class PcStateMachine
{
	private PcState CurrentState { get; set; }
	private Dictionary<PcStateNames, PcState> StatesByEnum { get; } = new();
	
	public PcStateMachine(PcStateContext context)
	{
		SetupStates(context);

		CurrentState = StatesByEnum[PcStateNames.MOVEMENT];
	}
	
	public void ProcessStateUnselected(float delta)
	{
		CurrentState?.ProcessUnselected(delta);
	}
	
	public void PhysicsProcessStateUnselected(float delta)
	{
		CurrentState?.PhysicsProcessUnselected(delta);
	}
	
	public void ProcessStateSelected(float delta)
	{
		CurrentState?.ProcessSelected(delta);
	}
	
	public void PhysicsProcessStateSelected(float delta)
	{
		CurrentState?.PhysicsProcessSelected(delta);
	}

	public void ChangeState(PcStateNames newStateName, Node3D target)
	{
		CurrentState?.ExitState();
		CurrentState = StatesByEnum[newStateName];
		CurrentState.Target = target;
		CurrentState?.EnterState();
	}
	
	public void MoveTo(Node3D target, Vector3 targetPosition = new())
	{
		this.PrintDebug($"Target type: {target?.GetType()}");
		if (target is LootContainer lootContainer)
		{
			targetPosition = lootContainer.LootingPosition;
		}
		if (CurrentState is PcStateMovement movementState)
		{
			movementState.Target = target;
			movementState.SetTargetPosition(targetPosition);
			return;
		}
		CurrentState?.ExitState();
		PcStateMovement movement = (PcStateMovement)StatesByEnum[PcStateNames.MOVEMENT];
		movement.Target = target;
		movement.SetTargetPosition(targetPosition);
		CurrentState = movement;
		CurrentState?.EnterState();		
	}
	
	public void ExitTree()
	{
		foreach (PcState state in StatesByEnum.Values)
		{
			state.OnChangeState -= ChangeState;
		}
	}

	private void SetupStates(PcStateContext context)
	{
		PcStateIdle idle = new(context);
		PcStateMovement movement = new(context);
		PcStateLooting looting = new(context);

		// Connect godot signal so no need to disconnect/unsubscribe later, so don't need to keep references to PC and movement state
		context.TargetDetector.Connect(
			Area3D.SignalName.BodyEntered,
			Callable.From((Node3D body) => movement.OnBodyEntered(body)));

		// Populate states dictionary
		StatesByEnum.Add(PcStateNames.IDLE, idle);
		StatesByEnum.Add(PcStateNames.MOVEMENT, movement);
		StatesByEnum.Add(PcStateNames.LOOTING, looting);
		foreach (PcState state in StatesByEnum.Values)
		{
			state.OnChangeState += ChangeState;
		}
	}
}
