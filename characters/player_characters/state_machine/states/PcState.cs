using System;

namespace Lastdew
{	
	public abstract class PcState(PlayerCharacter pc)
    {
		public event Action<PcStateNames> OnChangeState;

		private const float UNARMED_ATTACK_RANGE = 1f;
		
        protected PlayerCharacter Pc { get; } = pc;
        protected static string BlendAmountPath => "parameters/movement_blend_tree/idle_move/blend_amount";
        
		/// <summary>
		/// Degrees per second
		/// </summary>
		protected static float TurnSpeed => 360f;

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

		protected float AttackRadius()
		{
			return Pc.Equipment.Weapon?.Range ?? UNARMED_ATTACK_RANGE;
		}
	}
}
