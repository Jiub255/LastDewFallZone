using Godot;
using System;

namespace Lastdew
{
	public class PcStateIncapacitated : PcCombatSubstate
	{
		private const string ATTACK_ANIM_NAME = "CharacterArmature|Death";
		private AnimationNodeStateMachinePlayback StateMachine { get; set; }
		
		public PcStateIncapacitated(PcStateContext context) : base(context)
		{
			StateMachine = (AnimationNodeStateMachinePlayback)PcAnimationTree.Get("parameters/playback");
		}
		
		public override void EnterState(Enemy target)
		{
			base.EnterState(target);

			StateMachine.Travel(ATTACK_ANIM_NAME);
		}

		public override void GetHit(Enemy attacker){}
	}
}
