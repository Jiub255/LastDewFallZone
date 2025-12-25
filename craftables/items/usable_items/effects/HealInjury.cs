using Godot;

namespace Lastdew
{
	[GlobalClass, Tool]
	public partial class HealInjury : Effect
	{
		[Export]
		public int HealAmount { get; private set; }
		
		public override string Description => $"Heals {HealAmount} injury";
		public override string Abbreviation => "H.I.";
		
		public override void ApplyEffect(PlayerCharacter pc)
		{
			pc.StatManager.Health.HealInjury(HealAmount);
		}
	}
}
