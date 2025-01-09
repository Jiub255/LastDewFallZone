using Godot;

namespace Lastdew
{
	/// <summary>
	/// TODO: Maybe just have pain killers relieve pain until the end of the scavenging/defense mission?
	/// No reason to have them so limited.
	/// </summary>
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
}
