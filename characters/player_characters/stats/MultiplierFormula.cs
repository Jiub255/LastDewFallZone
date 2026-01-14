using Godot;
using System;

namespace Lastdew
{
	[GlobalClass]
	public partial class MultiplierFormula : ExperienceFormula
	{
		[Export] private double Multiplier { get; set; } = 5f;
		
		// Multiplier * (level - 1) ^ 2 = exp  =>  level = sqrt(exp / Multiplier) + 1
		// Shifted it up a level so 0 exp -> lvl 1.
		public override int LevelFromExperience(long exp)
		{
			return (int)Math.Floor(Math.Sqrt(exp / Multiplier)) + 1;
		}

		public override (long startLevelExp, long nextLevelExp) ExperienceRangeFromLevel(int level)
		{
			long startLevelExp = (long)Math.Floor(Multiplier * (level - 1) * (level - 1));
			long nextLevelExp = (long)Math.Floor(Multiplier * level * level);
			return (startLevelExp, nextLevelExp);
		}
	}
}
