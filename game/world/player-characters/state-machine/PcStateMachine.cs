using Godot;
using System.Collections.Generic;

namespace Lastdew
{	
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
	
		public void ChangeState(PcStateNames stateName, MovementTarget target)
		{
			this.PrintDebug($"Changing to {stateName}");
			CurrentState?.ExitState();
			CurrentState = StatesByEnum[stateName];
			CurrentState?.EnterState(target);
		}
	
		public void HitEnemy()
		{
			if (CurrentState is PcStateCombat combat)
			{
				combat.HitEnemy();
			}
			else
			{
				GD.PushWarning($"PC not in combat state. Current state is {CurrentState.GetType()}");
			}
		}
	
		public void GetHit(Enemy attacker)
		{
			this.PrintDebug($"State machine GetHit called");
			if (CurrentState is not PcStateCombat)
			{
				ChangeState(PcStateNames.COMBAT, new MovementTarget(Vector3.Zero, attacker));
			}
			PcStateCombat combat = (PcStateCombat)CurrentState;
			combat.GetHit(attacker);
		}
	
		public void ExitTree()
		{
			foreach (PcState state in StatesByEnum.Values)
			{
				state.OnChangeState -= ChangeState;
			}
			PcStateCombat combat = (PcStateCombat)StatesByEnum[PcStateNames.COMBAT];
			combat.ExitTree();
		}
	
		private void SetupStates(PcStateContext context)
		{
			PcStateIdle idle = new(context);
			PcStateMovement movement = new(context);
			PcStateLooting looting = new(context);
			PcStateCombat combat = new(context);
	
			// Populate states dictionary
			StatesByEnum.Add(PcStateNames.IDLE, idle);
			StatesByEnum.Add(PcStateNames.MOVEMENT, movement);
			StatesByEnum.Add(PcStateNames.LOOTING, looting);
			StatesByEnum.Add(PcStateNames.COMBAT, combat);
			
			foreach (PcState state in StatesByEnum.Values)
			{
				state.OnChangeState += ChangeState;
			}
		}
	}
}
