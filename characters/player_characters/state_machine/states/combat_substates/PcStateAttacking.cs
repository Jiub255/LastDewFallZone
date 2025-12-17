using Godot;

namespace Lastdew
{
	public class PcStateAttacking : PcCombatSubstate
	{
		private const string MOVEMENT_BLEND_TREE = "movement_blend_tree";
		private const string ATTACK_STATE_MACHINE = "AttackStateMachine";
		private const string PUNCH_ANIM_NAME = "CharacterArmature|Punch_Right";
		private const string MELEE_ANIM_NAME = "CharacterArmature|Sword_Slash";
		private const string SHOOT_ANIM_NAME = "CharacterArmature|Gun_Shoot";
		
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

			Pc.AnimStateMachine.Travel(ATTACK_STATE_MACHINE);
			AnimationNodeStateMachinePlayback playback =
				(AnimationNodeStateMachinePlayback)Pc.PcAnimationTree.Get(
					$"parameters/{ATTACK_STATE_MACHINE}/playback");
			if (Pc.Equipment.Weapon == null)
			{
				playback.Travel(PUNCH_ANIM_NAME);
			}
			else switch (Pc.Equipment.Weapon.WeaponType)
			{
				case WeaponType.MELEE:
					playback.Travel(MELEE_ANIM_NAME);
					break;
				case WeaponType.GUN:
					playback.Travel(SHOOT_ANIM_NAME);
					break;
				default:
					GD.PushError($"Can't process weapon type {Pc.Equipment.Weapon.WeaponType}");
					break;
			}
		}

		public override void ExitState()
		{
			Pc.AnimStateMachine.Travel(MOVEMENT_BLEND_TREE);
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
			bool killedEnemy = enemy.GetHit(attack, Pc);
			if (killedEnemy)
			{
				Pc.StatManager.Experience.GainExperience(enemy.Data.Experience);
			}
			//this.PrintDebug("Enemy.GetHit() called.");
		}
		
		private void OnAnimationFinished(string animationName)
		{
			if (animationName is PUNCH_ANIM_NAME or MELEE_ANIM_NAME or SHOOT_ANIM_NAME)
			{
				ChangeSubstate(PcCombatSubstateNames.WAITING);
			}
		}
	}
}
