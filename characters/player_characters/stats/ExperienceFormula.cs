using Godot;
using System;

namespace Lastdew
{
	[GlobalClass]
	public partial class ExperienceFormula : Resource
	{
		[Export] private float Multiplier { get; set; } = 5f;
		
		// Multiplier * (level - 1) ^ 2 = exp  =>  level = sqrt(exp / Multiplier) + 1
		// Shifted it up a level so 0 exp -> lvl 1.
		public int LevelFromExperience(int exp)
		{
			return (int)Math.Floor(MathF.Sqrt(exp / Multiplier)) + 1;
		}

		public (int startLevelExp, int nextLevelExp) ExperienceRangeFromLevel(int level)
		{
			int startLevelExp = Mathf.FloorToInt(Multiplier * (level - 1) * (level - 1));
			int nextLevelExp = Mathf.FloorToInt(Multiplier * level * level);
			return (startLevelExp, nextLevelExp);
		}
	}
}
