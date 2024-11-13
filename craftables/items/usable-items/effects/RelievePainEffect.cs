using Godot;

[GlobalClass]
public partial class RelievePainEffect : Effect
{
	[Export]
	public int ReliefAmount { get; set; }
	[Export]
	public float Duration { get; set; }
	
	public override void ApplyEffect(PlayerCharacter pc)
	{
		pc.Health.RelievePain(ReliefAmount, Duration);
	}
}
