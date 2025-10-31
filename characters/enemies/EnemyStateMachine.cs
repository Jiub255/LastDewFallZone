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
		
		public void ChangeState(EnemyStateNames stateName)
		{
			//this.PrintDebug($"Changing to {stateName}");
			CurrentState = StatesByEnum[stateName];
			CurrentState?.EnterState();
		}
		
		public void ExitTree()
		{
			foreach (EnemyState state in StatesByEnum.Values)
			{
				state.OnChangeState -= ChangeState;
			}
		}
	
		private void SetupStates(Enemy enemy)
		{
			EnemyStateIdle idle = new(enemy);
			EnemyStateMovement movement = new(enemy);
			EnemyStateCombat combat = new(enemy);
			EnemyStateDead dead = new(enemy);
	
			StatesByEnum.Add(EnemyStateNames.IDLE, idle);
			StatesByEnum.Add(EnemyStateNames.MOVEMENT, movement);
			StatesByEnum.Add(EnemyStateNames.COMBAT, combat);
			StatesByEnum.Add(EnemyStateNames.DEAD, dead);
			
			foreach (EnemyState state in StatesByEnum.Values)
			{
				state.OnChangeState += ChangeState;
			}
		}
	}
}
