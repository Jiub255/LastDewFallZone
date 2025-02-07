
namespace Lastdew
{	
	public class PcStateLooting : PcState
	{
		private const string LOOTING_ANIM_NAME = "CharacterArmature|Loot";
		private const string MOVEMENT_BLEND_TREE_NAME = "movement_blend_tree";
		
		public PcStateLooting(PcStateContext context) : base(context) {}
		
		public LootContainer LootContainer { get; private set; }
		private float Timer { get; set; }
	
		public override void EnterState(MovementTarget target)
		{
			if (target.Target is LootContainer lootContainer)
			{
				LootContainer = lootContainer;
				if (lootContainer.Empty || lootContainer.BeingLooted)
				{
					ChangeState(PcStateNames.IDLE, new MovementTarget(Context.Position));
					return;
				}
				lootContainer.BeingLooted = true;
				Context.AnimStateMachine.Travel(LOOTING_ANIM_NAME);
				Timer = lootContainer.LootDuration;
			}
		}
	
		public override void ExitState()
		{
			LootContainer.BeingLooted = false;
			Context.AnimStateMachine.Travel(MOVEMENT_BLEND_TREE_NAME);
		}
	
		public override void ProcessUnselected(float delta)
		{
			Context.RotateToward(LootContainer.GlobalPosition, TurnSpeed * delta);
			TickTimer(delta);
		}
	
		public override void ProcessSelected(float delta)
		{
			Context.RotateToward(LootContainer.GlobalPosition, TurnSpeed * delta);
			TickTimer(delta);
		}
	
		public override void PhysicsProcessUnselected(float delta) {}
		public override void PhysicsProcessSelected(float delta) {}
	
		private void TickTimer(float delta)
		{
			this.PrintDebug($"Timer: {Timer}");
			Timer -= delta;
			this.PrintDebug($"Timer: {Timer}");
			if (Timer < 0)
			{
				this.PrintDebug($"Inside if block, Timer: {Timer}");
				Timer = 0;
				GimmeTheLoot();
				ChangeState(PcStateNames.IDLE, new MovementTarget(Context.Position));
			}
		}
	
		private void GimmeTheLoot()
		{
			this.PrintDebug($"Gimme the loot, number of loot items: {LootContainer.Loot.Length}");
			foreach (ItemAmount itemAmount in LootContainer.Loot)
			{
				this.PrintDebug($"Looting {itemAmount.Item.Name}");
				Context.InventoryManager.AddItems(itemAmount.Item, itemAmount.Amount);
			}
			LootContainer.Empty = true;
		}
	}
}
