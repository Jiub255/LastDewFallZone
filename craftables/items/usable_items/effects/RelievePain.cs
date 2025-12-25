using Godot;

namespace Lastdew
{
	/// <summary>
	/// TODO: Maybe just have pain killers relieve pain until the end of the scavenging/defense mission?
	/// No reason to have them so limited.
	/// </summary>
	[GlobalClass, Tool]
	public partial class RelievePain : Effect
	{
		[Export]
		public int ReliefAmount { get; private set; }
		[Export]
		public float Duration { get; private set; }

        public override string Description => $"Relieves {ReliefAmount} pain for {Duration}s";
        public override string Abbreviation => "R.P.";

        public override void ApplyEffect(PlayerCharacter pc)
		{
			pc.StatManager.Health.RelievePain(ReliefAmount, Duration);
		}
	}
}
