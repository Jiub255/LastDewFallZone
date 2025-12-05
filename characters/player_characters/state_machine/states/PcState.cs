using System;

namespace Lastdew
{	
	public abstract class PcState(PlayerCharacter pc)
    {
		public event Action<PcStateNames> OnChangeState;

        protected PlayerCharacter Pc { get; private set; } = pc;
        protected static string BlendAmountPath => "parameters/movement_blend_tree/idle_move/blend_amount";

        /// <summary>
		/// TODO: Get this from PC weapon/stats eventually.
		/// </summary>
		protected static float AttackRadius => 1f;

		/// <summary>
		/// Degrees per second
		/// </summary>
		protected float TurnSpeed { get; set; } = 360f;

        public abstract void EnterState();
		public abstract void ExitState();
		public abstract void ProcessUnselected(float delta);
		public abstract void PhysicsProcessUnselected(float delta);
		public abstract void ProcessSelected(float delta);
		public abstract void PhysicsProcessSelected(float delta);
		
		protected void ChangeState(PcStateNames stateName)
		{
			OnChangeState?.Invoke(stateName);
		}
	}
}
