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
		
		/// <returns>true if hit killed enemy</returns>
		public bool HitEnemy()
		{
			if (Pc.MovementTarget.Target is Enemy enemy)
			{
				int attack = Pc.StatManager.Attack;
	            //this.PrintDebug($"Attack: {attack}");
	            return enemy.GetHit(attack, Pc);
			}
			return false;
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
