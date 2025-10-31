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
			
			Pc.RotateToward(Target.GlobalPosition, TurnSpeed * delta);
		}

		public override void ProcessUnselected(float delta)
		{
			base.ProcessUnselected(delta);
			
			Pc.RotateToward(Target.GlobalPosition, TurnSpeed * delta);
		}

		public override void EnterState(Enemy target)
		{
			base.EnterState(target);

			Pc.AnimStateMachine.Travel(ATTACK_ANIM_NAME);
		}
 
		public override void GetHit()
		{
			ChangeSubstate(PcCombatSubstateNames.GETTING_HIT);
		}
		
		/// <returns>true if hit killed enemy</returns>
		public bool HitEnemy(PlayerCharacter attackingPc)
		{
			int attack = Pc.StatManager.Attack;
            this.PrintDebug($"Attack: {attack}");
            return Target.GetHit(attack, attackingPc);
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
