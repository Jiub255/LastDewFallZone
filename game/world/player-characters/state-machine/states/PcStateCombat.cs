using Godot;
using System;
using System.Collections.Generic;

namespace Lastdew
{
	// TODO: Split this state into substates? Like attacking, (combat) idle, getting hit, dying?
	// Might make controls easier, ie no attack input while attacking or getting hit.
	// Could just use PcState for the substates, then hold CurrentSubstate in this state, and run the processes through it. 
	// Have change state stuff too obviously. 
	public partial class PcStateCombat : PcState<PcStateNames>
	{
		private Enemy Target { get; set; }
		private int AttackPower { get; } = 1;
		private float TimeBetweenAttacks { get; } = 2.3f;
		private float Timer { get; set; }
		private PcState<PcCombatSubstateNames> Substate { get; set; }
		private Dictionary<PcCombatSubstateNames, PcState<PcCombatSubstateNames>> StatesByEnum { get; } = new();
		
		public PcStateCombat(PcStateContext context) : base(context)
		{
			SetupStates(context);
		}
	
		public override void EnterState(MovementTarget target)
		{
			if (target.Target is Enemy enemy)
			{
				Target = enemy;
			}
		}

		public override void ProcessSelected(float delta)
		{
			Fight(delta);
			Substate.ProcessSelected(delta);
		}
	
		public override void ProcessUnselected(float delta)
		{
			Fight(delta);
			Substate.ProcessUnselected(delta);
		}

		public override void PhysicsProcessSelected(float delta)
		{
			Substate.PhysicsProcessSelected(delta);
		}
		
		public override void PhysicsProcessUnselected(float delta)
		{
			Substate.PhysicsProcessUnselected(delta);
		}
		
		public override void ExitState() {}
		
		public void ExitTree()
		{			
			foreach (PcState<PcCombatSubstateNames> state in StatesByEnum.Values)
			{
				state.OnChangeState -= ChangeSubstate;
			}
		}
		
		public void HitEnemy()
		{
			this.PrintDebug("HitEnemy called");
			// Play enemy get hit animation
			// Take enemy health
			Target.GetHit(AttackPower);
		}
		
		public void GetHit()
		{
			// Play PC get hit animation
			Context.PcAnimationTree.Set("parameters/conditions/GettingHit", true);
			/* Context.PcAnimationTree.CallDeferred(
				AnimationTree.MethodName.Set,
				"parameters/conditions/GettingHit",
				false); */
			this.PrintDebug("Combat state GetHit called");
		}

		private void Fight(float delta)
		{
			Timer -= delta;
			if (Timer < 0)
			{
				Timer = TimeBetweenAttacks;
				Attack();
			}
		}
	
		private void Attack()
		{
			this.PrintDebug($"Pc attack called");
			// Play PC attack animation
			Context.PcAnimationTree.Set("parameters/conditions/Attacking", true);
			/* Context.PcAnimationTree.CallDeferred(
				AnimationTree.MethodName.Set,
				"parameters/conditions/Attacking",
				false); */
		}
	
		private void SetupStates(PcStateContext context)
		{
			PcStateWaiting waiting = new(context);
			PcStateAttacking attacking = new(context);
			PcStateGettingHit gettingHit = new(context);
			
			// Populate states dictionary
			StatesByEnum.Add(PcCombatSubstateNames.WAITING, waiting);
			StatesByEnum.Add(PcCombatSubstateNames.ATTACKING, attacking);
			StatesByEnum.Add(PcCombatSubstateNames.GETTING_HIT, gettingHit);
			
			foreach (PcState<PcCombatSubstateNames> state in StatesByEnum.Values)
			{
				state.OnChangeState += ChangeSubstate;
			}
		}
		
		private void ChangeSubstate(PcCombatSubstateNames substateName, MovementTarget target)
		{
			this.PrintDebug($"Changing to {substateName}");
			Substate?.ExitState();
			Substate = StatesByEnum[substateName];
			Substate?.EnterState(target);
		}
	}
}
