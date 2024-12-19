using System;

namespace Lastdew
{	
	public abstract class PcState<T> where T : Enum
	{
		public event Action<T, MovementTarget> OnChangeState;
		
		protected PcStateContext Context { get; private set; }
		protected string BlendAmountPath { get; } = "parameters/movement_blend_tree/idle_move/blend_amount";
		/// <summary>
		/// TODO: Get this from PC weapon/stats eventually.
		/// </summary>
		protected float AttackRadius { get; } = 1f;
		
		public PcState(PcStateContext context)
		{
			Context = context;
		}
		
		public abstract void EnterState(MovementTarget target);
		public abstract void ExitState();
		public abstract void ProcessUnselected(float delta);
		public abstract void PhysicsProcessUnselected(float delta);
		public abstract void ProcessSelected(float delta);
		public abstract void PhysicsProcessSelected(float delta);
		
		protected void ChangeState(T stateName, MovementTarget target)
		{
			OnChangeState?.Invoke(stateName, target);
		}
	}
}
