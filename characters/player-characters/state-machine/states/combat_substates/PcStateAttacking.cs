using Godot;

namespace Lastdew
{
	public partial class PcStateAttacking : PcCombatSubstate
	{
		private const string ATTACK_ANIM_NAME = "CharacterArmature|Punch_Right";
		
		public PcStateAttacking(PcStateContext context) : base(context)
		{
			context.PcAnimationTree.Connect(
				AnimationTree.SignalName.AnimationFinished,
				Callable.From((string animationName) => OnAnimationFinished(animationName)));
		}

		public override void ProcessSelected(float delta)
		{
			base.ProcessSelected(delta);
			
			Context.RotateToward(Target.GlobalPosition, TurnSpeed * delta);
		}

		public override void ProcessUnselected(float delta)
		{
			base.ProcessUnselected(delta);
			
			Context.RotateToward(Target.GlobalPosition, TurnSpeed * delta);
		}

		public override void EnterState(Enemy target)
		{
			base.EnterState(target);

			Context.AnimStateMachine.Travel(ATTACK_ANIM_NAME);
		}
 
		public override void GetHit()
		{
			ChangeSubstate(PcCombatSubstateNames.GETTING_HIT);
		}
		
		/// <returns>true if hit killed enemy</returns>
		public bool HitEnemy(PlayerCharacter attackingPC)
		{
			int attack = Context.Attack;
            this.PrintDebug($"Attack: {attack}");
            return Target.GetHit(attack, attackingPC);
		}
		
		private void OnAnimationFinished(string animationName)
		{
			if (animationName == ATTACK_ANIM_NAME)
			{
				ChangeSubstate(PcCombatSubstateNames.WAITING);
			}
		}
	}
}
