using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class PcStateCombat : PcState
	{
		private PcCombatSubstate CurrentSubstate { get; set; }
		private Dictionary<PcCombatSubstateNames, PcCombatSubstate> StatesByEnum { get; } = new();
		private Enemy Target { get; set; }
		
		public PcStateCombat(PcStateContext context) : base(context)
		{
			SetupStates(context);
		}
	
		public override void EnterState(MovementTarget target)
		{
			if (target.Target is Enemy enemy)
			{
				Target = enemy;
				ChangeSubstate(PcCombatSubstateNames.WAITING);
			}
		}

		public override void ProcessSelected(float delta)
		{
			CurrentSubstate.ProcessSelected(delta);
		}
	
		public override void ProcessUnselected(float delta)
		{
			CurrentSubstate.ProcessUnselected(delta);
		}

		public override void PhysicsProcessSelected(float delta) {}
		
		public override void PhysicsProcessUnselected(float delta) {}
		
		public override void ExitState() {}
		
		public void ExitTree()
		{			
			foreach (PcCombatSubstate state in StatesByEnum.Values)
			{
				state.OnChangeSubstate -= ChangeSubstate;
			}
		}
		
		public void HitEnemy(PlayerCharacter attackingPC)
		{
			if (CurrentSubstate is PcStateAttacking attackState)
			{
				bool targetKilled = attackState.HitEnemy(attackingPC);
				if (targetKilled)
				{
					Enemy nearest = Context.FindNearestEnemy(Target);
					if (nearest != null)
					{
						ChangeState(PcStateNames.MOVEMENT, new MovementTarget(nearest.GlobalPosition, nearest));
					}
					else
					{
						ChangeState(PcStateNames.IDLE, new MovementTarget());
					}
				}
			}
			else
			{
				GD.PushWarning($"Not in attacking substate. Current substate: {CurrentSubstate.GetType()}");
			}
		}
		
		public void GetHit(Enemy attacker, bool incapacitated)
		{
			if (incapacitated)
			{
				ChangeSubstate(PcCombatSubstateNames.INCAPACITATED);
			}
			else
			{
				Target ??= attacker;
				CurrentSubstate.GetHit();
			}
		}
	
		private void SetupStates(PcStateContext context)
		{
			PcStateWaiting waiting = new(context);
			PcStateAttacking attacking = new(context);
			PcStateGettingHit gettingHit = new(context);
			PcStateIncapacitated incapacitated = new(context);
			
			// Populate states dictionary
			StatesByEnum.Add(PcCombatSubstateNames.WAITING, waiting);
			StatesByEnum.Add(PcCombatSubstateNames.ATTACKING, attacking);
			StatesByEnum.Add(PcCombatSubstateNames.GETTING_HIT, gettingHit);
			StatesByEnum.Add(PcCombatSubstateNames.INCAPACITATED, incapacitated);
			
			foreach (PcCombatSubstate state in StatesByEnum.Values)
			{
				state.OnChangeSubstate += ChangeSubstate;
			}
		}
		
		private void ChangeSubstate(PcCombatSubstateNames substateName)
		{
			CurrentSubstate = StatesByEnum[substateName];
			CurrentSubstate?.EnterState(Target);
		}
	}
}
