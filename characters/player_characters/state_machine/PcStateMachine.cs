using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public class PcStateMachine
	{
		public PcState CurrentState { get; private set; }
		private Dictionary<PcStateNames, PcState> StatesByEnum { get; } = [];
	
		public PcStateMachine(PlayerCharacter pc)
		{
			SetupStates(pc);
	
			CurrentState = StatesByEnum[PcStateNames.IDLE];
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
	
		public void ChangeState(PcStateNames stateName)
		{
			//this.PrintDebug($"Changing from {CurrentState.GetType()} to {stateName}");
			CurrentState?.ExitState();
			CurrentState = StatesByEnum[stateName];
			CurrentState?.EnterState();
		}
	
		public void HitEnemy()
		{
			if (CurrentState is not PcStateCombat combat)
			{
				GD.PushWarning($"PC not in combat state. Current state is {CurrentState.GetType()}");
				return;
			}
			combat.HitEnemy();
		}
	
		public void GetHit(bool incapacitated)
		{
			if (CurrentState is not PcStateCombat)
			{
				ChangeState(PcStateNames.COMBAT);
			}
			PcStateCombat combat = (PcStateCombat)CurrentState;
			combat.GetHit(incapacitated);
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
	
		private void SetupStates(PlayerCharacter pc)
		{
			PcStateIdle idle = new(pc);
			PcStateMovement movement = new(pc);
			PcStateLooting looting = new(pc);
			PcStateCombat combat = new(pc);
	
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
