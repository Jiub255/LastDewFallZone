using System.Collections.Generic;
using Godot;
using Godot.NativeInterop;

namespace Lastdew
{	
	public class PcStateLooting(PlayerCharacter pc) : PcState(pc)
	{
		private const string LOOTING_ANIM_NAME = "CharacterArmature|Loot";
		private const string MOVEMENT_BLEND_TREE_NAME = "movement_blend_tree";

		private LootContainer LootContainer { get; set; }
		private float Timer { get; set; }
	
		public override void EnterState()
		{
			if (Pc.MovementTarget.Target is LootContainer lootContainer)
			{
				LootContainer = lootContainer;
				if (lootContainer.Empty || lootContainer.BeingLooted)
				{
					ChangeState(PcStateNames.IDLE);
					return;
				}
				lootContainer.BeingLooted = true;
				Pc.AnimStateMachine.Travel(LOOTING_ANIM_NAME);
				Timer = lootContainer.LootDuration;
			}
			else
			{
				GD.PushWarning("Movement target wasn't a loot container.");
				ChangeState(PcStateNames.IDLE);
			}
		}
	
		public override void ExitState()
		{
			LootContainer.BeingLooted = false;
			Pc.AnimStateMachine.Travel(MOVEMENT_BLEND_TREE_NAME);
		}
	
		public override void ProcessUnselected(float delta)
		{
			Pc.RotateToward(LootContainer.GlobalPosition, TurnSpeed * delta);
			TickTimer(delta);
		}
	
		public override void ProcessSelected(float delta)
		{
			Pc.RotateToward(LootContainer.GlobalPosition, TurnSpeed * delta);
			TickTimer(delta);
		}
	
		public override void PhysicsProcessUnselected(float delta) {}
		public override void PhysicsProcessSelected(float delta) {}
	
		private void TickTimer(float delta)
		{
			Timer -= delta;
			if (Timer < 0)
			{
				Timer = 0;
				GimmeTheLoot();
				ChangeState(PcStateNames.IDLE);
			}
		}
	
		private void GimmeTheLoot()
		{
			foreach (Item item in LootContainer.Loot)
			{
				Pc.CollectLoot(item);
			}
			Pc.StatManager.Experience.GainExperience(LootContainer.Experience);
			LootContainer.Empty = true;
		}
	}
}
