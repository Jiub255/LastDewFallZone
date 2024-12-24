using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class PcStateCombat : PcState
	{
		private PcCombatSubstate CurrentSubstate { get; set; }
		private Dictionary<PcCombatSubstateNames, PcCombatSubstate> StatesByEnum { get; } = new();
		
		public PcStateCombat(PcStateContext context) : base(context)
		{
			SetupStates(context.PcAnimationTree);
		}
	
		public override void EnterState(MovementTarget target)
		{
			if (target.Target is Enemy enemy)
			{
				ChangeSubstate(PcCombatSubstateNames.WAITING, enemy);
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
		
		public void HitEnemy()
		{
			if (CurrentSubstate is PcStateAttacking attack)
			{
				attack.HitEnemy();
			}
			else
			{
				GD.PushWarning($"Not in attacking substate. Current substate: {CurrentSubstate.GetType()}");
			}
		}
		
		public void GetHit(Enemy attacker)
		{
			CurrentSubstate.GetHit(attacker);
		}
	
		private void SetupStates(PcAnimationTree pcAnimationTree)
		{
			PcStateWaiting waiting = new(pcAnimationTree);
			PcStateAttacking attacking = new(pcAnimationTree);
			PcStateGettingHit gettingHit = new(pcAnimationTree);
			
			// Populate states dictionary
			StatesByEnum.Add(PcCombatSubstateNames.WAITING, waiting);
			StatesByEnum.Add(PcCombatSubstateNames.ATTACKING, attacking);
			StatesByEnum.Add(PcCombatSubstateNames.GETTING_HIT, gettingHit);
			
			foreach (PcCombatSubstate state in StatesByEnum.Values)
			{
				state.OnChangeSubstate += ChangeSubstate;
			}
		}
		
		private void ChangeSubstate(PcCombatSubstateNames substateName, Enemy target)
		{
			this.PrintDebug($"Changing to {substateName}");
			CurrentSubstate?.ExitState();
			CurrentSubstate = StatesByEnum[substateName];
			CurrentSubstate?.EnterState(target);
		}
	}
}
