public class PcStateLooting : PcState
{
	public PcStateLooting(PcStateContext context) : base(context) {}
	
	public LootContainer LootContainer { get; private set; }
	private float Timer { get; set; }

	public override void EnterState(object target)
	{
		if (target is LootContainer lootContainer)
		{
			LootContainer = lootContainer;
			if (lootContainer.Empty || lootContainer.BeingLooted)
			{
				ChangeState(PcStateNames.IDLE);
				return;
			}
			lootContainer.BeingLooted = true;
			Context.PcAnimationTree.Looting = true;
			Timer = lootContainer.LootDuration;
		}
	}

	public override void ExitState()
	{
		Context.PcAnimationTree.Looting = false;
		LootContainer.BeingLooted = false;
	}

	public override void ProcessUnselected(float delta)
	{
		TickTimer(delta);
	}

	public override void ProcessSelected(float delta)
	{
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
		this.PrintDebug($"Gimme the loot, number of loot items: {LootContainer.Loot.Length}");
		foreach (ItemAmount itemAmount in LootContainer.Loot)
		{
			this.PrintDebug($"Looting {itemAmount.Item.Name}");
			Context.InventoryManager.AddItemAmount(itemAmount);
		}
		LootContainer.Empty = true;
	}
}
