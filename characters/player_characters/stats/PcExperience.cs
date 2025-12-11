using System;

namespace Lastdew
{
	public class PcExperience
	{
		public event Action<string> OnExperienceGained;
		public event Action OnLevelUp;

		public int Experience { get; private set; }
		public int Level { get; private set; }
		

		public PcExperience(PcSaveData saveData)
		{
			Experience = saveData.Experience;
			Level = LevelFromExperience(Experience);
		}
		
		public void GainExperience(int experienceGained)
		{
			Experience += experienceGained;
			int levelsGained = LevelFromExperience(Experience) - Level;
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

		// 3 * (level - 1)^2 = exp  =>  level = sqrt(exp / 3) + 1
		// Shifting it up a level so 0 exp -> lvl 1.
		// exp		lvl
		// 0		1
		// 3		2
		// 12		4
		// 27		5
		// 48		6
		// private static int LevelFromExperience(int exp)
		// {
		// 	return (int)Math.Floor(MathF.Sqrt(exp / 3f)) + 1;
		// }

		// 5 * (level - 1)^2 = exp  =>  level = sqrt(exp / 5) + 1
		// Shifting it up a level so 0 exp -> lvl 1.
		// exp		lvl
		// 0		1
		// 5		2
		// 20		4
		// 45		5
		// 80		6
		private static int LevelFromExperience(int exp)
		{
			return (int)Math.Floor(MathF.Sqrt(exp / 5f)) + 1;
		}
	}
}
