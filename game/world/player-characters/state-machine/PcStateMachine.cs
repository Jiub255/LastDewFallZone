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
	
	public void ChangeState(PcStateNames stateName, object target = null)
	{
		this.PrintDebug($"Changing to {stateName}");
		CurrentState?.ExitState();
		CurrentState = StatesByEnum[stateName];
		CurrentState?.EnterState(target);
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
