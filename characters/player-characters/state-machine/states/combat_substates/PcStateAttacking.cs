using Godot;

namespace Lastdew
{
	public class PcStateAttacking : PcCombatSubstate
	{
		private const string ATTACK_ANIM_NAME = "CharacterArmature|Punch_Right";
		
		public PcStateAttacking(PlayerCharacter pc) : base(pc)
		{
			pc.PcAnimationTree.Connect(
				AnimationMixer.SignalName.AnimationFinished,
				Callable.From((string animationName) => OnAnimationFinished(animationName)));
		}

		public override void ProcessSelected(float delta)
		{
			base.ProcessSelected(delta);
			
			Pc.RotateToward(Pc.MovementTarget.Target.GlobalPosition, TurnSpeed * delta);
		}

		public override void ProcessUnselected(float delta)
		{
			base.ProcessUnselected(delta);
			
			Pc.RotateToward(Pc.MovementTarget.Target.GlobalPosition, TurnSpeed * delta);
		}

		public override void EnterState()
		{
			base.EnterState();

			Pc.AnimStateMachine.Travel(ATTACK_ANIM_NAME);
		}
 
		public override void GetHit()
		{
			ChangeSubstate(PcCombatSubstateNames.GETTING_HIT);
		}
		
		public void HitEnemy()
		{
			if (Pc.MovementTarget.Target is not Enemy enemy)
			{
				GD.PushWarning($"Target is of type {Pc.MovementTarget.Target.GetType()}, not Enemy");
				return;
			}
			int attack = Pc.StatManager.Attack;
			enemy.GetHit(attack, Pc);
			//this.PrintDebug("Enemy.GetHit() called.");
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
