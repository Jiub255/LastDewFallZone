using Godot;
using System;

public partial class RelievePainEffect : Effect
{
	[Export]
	public int ReliefAmount { get; set; }
	[Export]
	public float Duration { get; set; }
	
	public override void ApplyEffect(PlayerCharacter pc)
	{
		//pc.PainInjury.RelievePain(ReliefAmount, Duration);
		throw new NotImplementedException();
	}
}
