using System;

namespace Lastdew
{
	public abstract class PcCombatSubstate
	{
		public event Action<PcCombatSubstateNames> OnChangeSubstate;
		
		protected PcStateContext Context { get; set; }
		protected Enemy Target { get; set; }
		protected float TimeBetweenAttacks { get; } = 2.3f;
		protected float Timer { get; set; }
		/// <summary>
		/// Degrees per second
		/// </summary>
		protected float TurnSpeed { get; set; } = 360f;
		
		
		protected PcCombatSubstate(PcStateContext context)
		{
			Context = context;
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
		
		public abstract void GetHit();
		
		protected virtual void Tick(float delta)
		{
			Timer -= delta;
		}

		protected void ChangeSubstate(PcCombatSubstateNames substateName)
		{
			OnChangeSubstate?.Invoke(substateName);
		}
	}
}
