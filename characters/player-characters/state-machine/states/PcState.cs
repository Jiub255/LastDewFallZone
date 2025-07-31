using System;

namespace Lastdew
{	
	public abstract class PcState(PcStateContext context)
    {
		public event Action<PcStateNames, MovementTarget> OnChangeState;

        protected PcStateContext Context { get; private set; } = context;
        protected string BlendAmountPath { get; } = "parameters/movement_blend_tree/idle_move/blend_amount";
		/// <summary>
		/// TODO: Get this from PC weapon/stats eventually.
		/// </summary>
		protected float AttackRadius { get; } = 0.5f;
		/// <summary>
		/// Degrees per second
		/// </summary>
		protected float TurnSpeed { get; set; } = 360f;

        public abstract void EnterState(MovementTarget target);
		public abstract void ExitState();
		public abstract void ProcessUnselected(float delta);
		public abstract void PhysicsProcessUnselected(float delta);
		public abstract void ProcessSelected(float delta);
		public abstract void PhysicsProcessSelected(float delta);
		
		protected void ChangeState(PcStateNames stateName, MovementTarget target)
		{
			OnChangeState?.Invoke(stateName, target);
		}
	}
}
