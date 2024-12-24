using System;

namespace Lastdew
{
	public abstract class PcCombatSubstate
	{
		public event Action<PcCombatSubstateNames, Enemy> OnChangeSubstate;
		
		protected PcAnimationTree PcAnimationTree { get; set; }
		protected Enemy Target { get; set; }
		protected float TimeBetweenAttacks { get; } = 2.3f;
		protected float Timer { get; set; }
		
		
		protected PcCombatSubstate(PcAnimationTree pcAnimationTree)
		{
			PcAnimationTree = pcAnimationTree;
		}

		public virtual void EnterState(Enemy target)
		{
			Target = target;
		}

		public virtual void ProcessSelected(float delta)
		{
			Tick(delta);
		}

		public virtual void ProcessUnselected(float delta)
		{
			Tick(delta);
		}

		public abstract void ExitState();
		
		public abstract void GetHit(Enemy attacker);
		
		protected virtual void Tick(float delta)
		{
			Timer -= delta;
		}

		protected void ChangeSubstate(PcCombatSubstateNames substateName, Enemy target)
		{
			OnChangeSubstate?.Invoke(substateName, target);
		}
	}
}
