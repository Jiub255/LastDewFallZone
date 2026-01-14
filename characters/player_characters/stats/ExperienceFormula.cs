using System;
using Godot;

namespace Lastdew
{
	public abstract partial class ExperienceFormula : Resource
	{
		public abstract int LevelFromExperience(long exp);
		public abstract (long startLevelExp, long nextLevelExp) ExperienceRangeFromLevel(int level);

		public void PrintLevels()
		{
			for (int level = 1; level <= 100; level++)
			{
				long xpDiff = ExperienceRangeFromLevel(level).startLevelExp - 
				             Math.Max(0, ExperienceRangeFromLevel(level - 1).startLevelExp);
				GD.Print($"Level {level}: {xpDiff} exp, {ExperienceRangeFromLevel(level).startLevelExp} total");
			}
		}
	}
}
