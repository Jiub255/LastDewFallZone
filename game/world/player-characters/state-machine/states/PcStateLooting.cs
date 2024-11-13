public class PcStateLooting : PcState
{
	public PcStateLooting(PcStateContext context) : base(context) {}
	
	private LootContainer LootContainer { get; set; }
	private float Timer { get; set; }

	public override void EnterState()
	{
		Context.PcAnimationTree.Looting = true;
		LootContainer = (LootContainer)Target;
		Timer = LootContainer.LootDuration;
	}

	public override void ExitState()
	{
		Context.PcAnimationTree.Looting = false;
	}

	public override void ProcessUnselected(float delta)
    {
        TickLootTimer(delta);
    }

	public override void ProcessSelected(float delta)
	{
		TickLootTimer(delta);
	}

    public override void PhysicsProcessUnselected(float delta) {}
	public override void PhysicsProcessSelected(float delta) {}

    private void TickLootTimer(float delta)
    {
        Timer -= delta;
        if (Timer < 0)
        {
            Timer = 0;
            // TODO: Add loot to inventory.
            // TODO: Change back to movement (idle) state.
        }
    }
}
