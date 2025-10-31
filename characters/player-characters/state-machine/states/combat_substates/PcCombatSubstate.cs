using System;

namespace Lastdew
{
	public abstract class PcCombatSubstate(PlayerCharacter pc)
    {
		public event Action<PcCombatSubstateNames> OnChangeSubstate;

        protected PlayerCharacter Pc { get; } = pc;
        protected Enemy Target { get; private set; }
		protected static float TimeBetweenAttacks => 2.3f;
		protected float Timer { get; set; }
		/// <summary>
		/// Degrees per second
		/// </summary>
		protected float TurnSpeed { get; set; } = 360f;

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
