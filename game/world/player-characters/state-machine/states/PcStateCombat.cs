using Godot;
using System;
using System.Collections.Generic;

namespace Lastdew
{
	// TODO: Split this state into substates? Like attacking, (combat) idle, getting hit, dying?
	// Might make controls easier, ie no attack input while attacking or getting hit.
	// Could just use PcState for the substates, then hold CurrentSubstate in this state, and run the processes through it. 
	// Have change state stuff too obviously. 
	public partial class PcStateCombat : PcState
	{
		private Enemy Target { get; set; }
		private int AttackPower { get; } = 1;
		private float TimeBetweenAttacks { get; } = 2.3f;
		private float Timer { get; set; }
		private PcState Substate { get; set; }
		private Dictionary<PcCombatSubstateNames, PcState> StatesByEnum { get; } = new();
		
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
		}
	
		public override void ProcessUnselected(float delta)
		{
			Fight(delta);
		}

		public override void PhysicsProcessSelected(float delta) {}
		public override void PhysicsProcessUnselected(float delta) {}
		public override void ExitState() {}
		
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
			
			foreach (PcState state in StatesByEnum.Values)
			{
				state.OnChangeState += ChangeSubstate;
			}
		}

		// TODO: How to handle different enum here? Add names to old enum? No.
		// Make new combatsubstate class? Probably.
		// OR, make PcState generic and take an enum? Might work?
		private void ChangeSubstate(PcStateNames names, MovementTarget target)
		{
			throw new NotImplementedException();
		}
	}
}
