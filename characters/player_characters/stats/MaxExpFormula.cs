using System;
using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class MaxExpFormula : ExperienceFormula
	{
		[Export] public int MaxLevel { get; set; } = 20;
		[Export] public int XpForFirstLevel { get; set; } = 10;
		[Export] public int XpForLastLevel { get; set; } = 69420;
		
		// exp = A * e ^ (B * level) <==>  (ln(exp / A)) / B = level
		private double B { get; set; }
		private double A { get; set; }
		
		public override int LevelFromExperience(long exp)
		{
			if (B == 0 && A == 0)
			{
				CalculateCoefficients();
			}

			return (int)Math.Floor(Math.Log(exp / A) / B);
		}

		public override (long startLevelExp, long nextLevelExp) ExperienceRangeFromLevel(int level)
		{
			if (B == 0 && A == 0)
			{
				CalculateCoefficients();
			}

			long startLevelExp = (long)Math.Floor(A * Math.Exp(B * (level - 1)));
			long nextLevelExp = (long)Math.Floor(A * Math.Exp(B * level));
			
			return (startLevelExp, nextLevelExp);
		}

		private void CalculateCoefficients()
		{
			B = Math.Log((double)XpForLastLevel / XpForFirstLevel) / (MaxLevel - 1);
			A = XpForFirstLevel * Math.Pow((double)XpForLastLevel / XpForFirstLevel, -2 / ((double)MaxLevel - 1));
		}
	}
}
