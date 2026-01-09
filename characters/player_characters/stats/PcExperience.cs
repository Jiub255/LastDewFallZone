using System;
using System.Numerics;
using Godot;

namespace Lastdew
{
	public class PcExperience
	{
		public event Action<string> OnExperienceGained;
		public event Action OnLevelUp;

		public int Experience { get; private set; }
		public int Level { get; private set; }
		private ExperienceFormula Formula { get; init; }
		

		public PcExperience(PcSaveData saveData, ExperienceFormula formula)
		{
			Experience = saveData.Experience;
			Formula = formula;
			
			Level = Formula.LevelFromExperience(Experience);
			//PrintLevels();
		}
		
		public void GainExperience(int experienceGained)
		{
			Experience += experienceGained;
			int levelsGained = Formula.LevelFromExperience(Experience) - Level;
			for (int i = 0; i < levelsGained; i++)
			{
				LevelUp();
			}
			OnExperienceGained?.Invoke($"{experienceGained} xp");
			//this.PrintDebug($"Gained {experienceGained} experience, total: {Experience}, level: {Level}");
		}

		private void LevelUp()
		{
			Level++;
			OnLevelUp?.Invoke();
		}


		// private void PrintLevels()
		// {
		// 	Godot.GD.Print("Level     Experience");
		// 	for (int i = 1; i < 101; i++)
		// 	{
		// 		Godot.GD.Print($"{i}         {5 * (i - 1) * (i - 1)}");
		// 	}
		// }
	}
}
