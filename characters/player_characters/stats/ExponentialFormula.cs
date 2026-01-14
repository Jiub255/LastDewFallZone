using System;
using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class ExponentialFormula : ExperienceFormula
	{
		[Export]
		public int MaxLevel { get; private set; } = 20;
		[Export]
		private long XpForLastLevel { get; set; } = 10000;
		/// <summary>
		/// experience == A * e ^ (K * level) + B <br></br>
		/// level == ln((experience - B) / A) / K
		/// </summary>
		[Export]
		private double K { get; set; } = 0.3d;
		
		private double A { get; set; }
		private double B { get; set; }
		private bool CoefficientsCalculated { get; set; }
		
		public override int LevelFromExperience(long exp)
		{
			if (!CoefficientsCalculated)
			{
				CalculateCoefficients();
			}

			return (int)Math.Floor(Math.Log((exp + 1 - B) / A) / K);
		}

		public override (long startLevelExp, long nextLevelExp) ExperienceRangeFromLevel(int level)
		{
			if (!CoefficientsCalculated)
			{
				CalculateCoefficients();
			}

			long startLevelExp = (long)Math.Floor(A * Math.Exp(K * level) + B);
			long nextLevelExp = (long)Math.Floor(A * Math.Exp(K * (level + 1)) + B);
			
			return (startLevelExp, nextLevelExp);
		}

		private void CalculateCoefficients()
		{
			A = XpForLastLevel / (Math.Exp(K * MaxLevel) - Math.Exp(K));
			B = -A * Math.Exp(K);
			CoefficientsCalculated = true;
			
			//GD.Print($"A: {A}, B: {B}");
		}
	}
}
