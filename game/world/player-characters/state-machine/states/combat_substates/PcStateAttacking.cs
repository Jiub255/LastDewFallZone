using Godot;

namespace Lastdew
{
	public partial class PcStateAttacking : PcCombatSubstate
	{
		private const string ATTACK_ANIM_NAME = "CharacterArmature|Punch_Right";
		private int AttackPower { get; } = 1;
		private AnimationNodeStateMachinePlayback StateMachine { get; set; }
		
		public PcStateAttacking(PcStateContext context) : base(context)
		{
			context.PcAnimationTree.Connect(
				AnimationTree.SignalName.AnimationFinished,
				Callable.From((string animationName) => OnAnimationFinished(animationName)));
			StateMachine = (AnimationNodeStateMachinePlayback)PcAnimationTree.Get("parameters/playback");
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

			StateMachine.Travel(ATTACK_ANIM_NAME);
		}
 
		public override void GetHit(Enemy attacker)
		{
			ChangeSubstate(PcCombatSubstateNames.GETTING_HIT, attacker);
		}
		
		public void HitEnemy()
		{
			Target.GetHit(AttackPower);
		}
		
		private void OnAnimationFinished(string animationName)
		{
			if (animationName == ATTACK_ANIM_NAME)
			{
				ChangeSubstate(PcCombatSubstateNames.WAITING, Target);
			}
		}
	}
}
