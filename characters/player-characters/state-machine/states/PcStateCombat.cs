using Godot;

namespace Lastdew
{
	public class PcStateCombat : PcState
	{
		private PcCombatSubstate CurrentSubstate { get; set; }
		private System.Collections.Generic.Dictionary<PcCombatSubstateNames, PcCombatSubstate> StatesByEnum { get; } = new();
		
		public PcStateCombat(PlayerCharacter pc) : base(pc)
		{
			SetupSubstates(pc);
		}
	
		public override void EnterState()
		{
			if (Pc.MovementTarget.Target is Enemy)
			{
				ChangeSubstate(PcCombatSubstateNames.WAITING);
			}
			else
			{
				ChangeState(PcStateNames.IDLE);
			}
		}

		public override void ProcessSelected(float delta)
		{
			bool pcIncapacitated = SharedProcess();
			if (pcIncapacitated) return;
			CurrentSubstate.ProcessSelected(delta);
		}

		public override void ProcessUnselected(float delta)
		{
			bool pcIncapacitated = SharedProcess();
			if (pcIncapacitated) return;
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
			if (CurrentSubstate is not PcStateAttacking attackState)
			{
				GD.PushWarning($"Not in attacking substate. Current substate: {CurrentSubstate.GetType()}");
				return;
			}
			attackState.HitEnemy();
		}

        public void GetHit(bool incapacitated)
		{
			if (incapacitated)
			{
				ChangeSubstate(PcCombatSubstateNames.INCAPACITATED);
				return;
			}
			CurrentSubstate.GetHit();
		}

		/// <returns>true if Pc.Incapacitated</returns>
		private bool SharedProcess()
		{
			if (Pc.Incapacitated)
			{
				return true;
			}
			ReapproachTargetIfOutOfRange();
			return false;
		}
	
		private void SetupSubstates(PlayerCharacter pc)
		{
			PcStateWaiting waiting = new(pc);
			PcStateAttacking attacking = new(pc);
			PcStateGettingHit gettingHit = new(pc);
			PcStateIncapacitated incapacitated = new(pc);
			
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
			CurrentSubstate?.EnterState();
		}


		private void ReapproachTargetIfOutOfRange()
		{
			//this.PrintDebug($"Distance: {Pc.GlobalPosition.DistanceTo(Pc.MovementTarget.Target.GlobalPosition)}");
			if (Pc.GlobalPosition.DistanceTo(Pc.MovementTarget.Target.GlobalPosition) >
			    AttackRadius * 1.5f)
			{
				ChangeState(PcStateNames.MOVEMENT);
			}
		}
	}
}
