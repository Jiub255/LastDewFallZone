using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public class EnemyStateMachine
	{
		private EnemyState CurrentState { get; set; }
		private Dictionary<EnemyStateNames, EnemyState> StatesByEnum { get; } = new();
	
		public EnemyStateMachine(Enemy enemy)
		{
			SetupStates(enemy);
	
			CurrentState = StatesByEnum[EnemyStateNames.IDLE];
		}
	
		public void ProcessState(float delta)
		{
			CurrentState?.ProcessState(delta);
		}
	
		public void PhysicsProcessState(float delta)
		{
			CurrentState?.PhysicsProcessState(delta);
		}
		
		public void ChangeState(EnemyStateNames stateName)
		{
			//this.PrintDebug($"Changing to {stateName}");
			CurrentState?.ExitState();
			CurrentState = StatesByEnum[stateName];
			CurrentState?.EnterState();
		}
		
		public void ExitTree()
		{
			foreach (EnemyState state in StatesByEnum.Values)
			{
				state.OnChangeState -= ChangeState;
			}
			// PcStateCombat combat = (PcStateCombat)StatesByEnum[PcStateNames.COMBAT];
			// combat.ExitTree();
		}
	
		private void SetupStates(Enemy enemy)
		{
			EnemyStateIdle idle = new(enemy);
			EnemyStateMovement movement = new(enemy);
			EnemyStateCombat combat = new(enemy);
	
			StatesByEnum.Add(EnemyStateNames.IDLE, idle);
			StatesByEnum.Add(EnemyStateNames.MOVEMENT, movement);
			StatesByEnum.Add(EnemyStateNames.COMBAT, combat);
			
			foreach (EnemyState state in StatesByEnum.Values)
			{
				state.OnChangeState += ChangeState;
			}
		}
	}
}
