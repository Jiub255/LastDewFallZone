using System;
using Godot;

namespace Lastdew
{
	[GlobalClass, Tool]
	public partial class HealInjury : Effect
	{
		private const int MAX_STAT_AMOUNT = 20;
		
		[Export]
		public int HealAmount { get; private set; }
		
		// TODO: Add bonuses from medical skill/buildings/items to the description.
		// Just display the total? Or show how much is base vs. bonus? Like "Heals 15 (10+5) injury"?
		public override string Description => $"Heals {HealAmount} injury";
		public override string Abbreviation => "H.I.";
		
		public override void ApplyEffect(TeamData teamData)
		{
			// TODO: Get highest medical stat and any medical buildings/items from team data/inventory.
			// How to work it into heal amount?
			// Also, it should show the correct numbers while in inventory.
			int highestMedical = teamData.MaximumStats[StatType.MEDICAL];
			// 1x multiplier at 0 medical, 2x multiplier at max medical.
			float multiplier = (float)highestMedical / MAX_STAT_AMOUNT + 1;
			int finalAmount = (int)Math.Round(HealAmount * multiplier);
			
			PlayerCharacter pc = teamData.Pcs[teamData.MenuSelectedIndex];
			pc.StatManager.Health.HealInjury(finalAmount);
		}
	}
}
