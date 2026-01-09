using Godot;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lastdew
{
	public class PcStatManager : IEnumerable<Stat>
	{
		private const int STAT_POINTS_GAINED_PER_LEVEL = 5;
		
		private readonly Stat _attack;
		private readonly Stat _defense;
		private readonly Stat _engineering;
		private readonly Stat _farming;
		private readonly Stat _medical;
		private readonly Stat _scavenging;

		public PcHealth Health { get; }
		public PcExperience Experience { get; }
		
		public int Attack => PainFormula(_attack.Value);
		public int Defense => PainFormula(_defense.Value);
		public int Engineering => PainFormula(_engineering.Value);
		public int Farming => PainFormula(_farming.Value);
		public int Medical => PainFormula(_medical.Value);
		public int Scavenging => PainFormula(_scavenging.Value);

		public int AttackBaseValue
		{
			get => _attack.BaseValue;
			set => _attack.BaseValue = value;
		}
		public int DefenseBaseValue
		{
			get => _defense.BaseValue;
			set => _defense.BaseValue = value;
		}
		public int EngineeringBaseValue
		{
			get => _engineering.BaseValue;
			set => _engineering.BaseValue = value;
		}
		public int FarmingBaseValue
		{
			get => _farming.BaseValue;
			set => _farming.BaseValue = value;
		}
		public int MedicalBaseValue
		{
			get => _medical.BaseValue;
			set => _medical.BaseValue = value;
		}
		public int ScavengingBaseValue
		{
			get => _scavenging.BaseValue;
			set => _scavenging.BaseValue = value;
		}
		
		private int StatPointsToSpend { get; set; }
		

		public PcStatManager(PcSaveData saveData, ExperienceFormula formula)
		{
			_attack = new Stat(StatType.ATTACK, saveData.Attack);
			_defense = new Stat(StatType.DEFENSE, saveData.Defense);
			_engineering = new Stat(StatType.ENGINEERING, saveData.Engineering);
			_farming = new Stat(StatType.FARMING, saveData.Farming);
			_medical = new Stat(StatType.MEDICAL, saveData.Medical);
			_scavenging = new Stat(StatType.SCAVENGING, saveData.Scavenging);
			Health = new PcHealth(saveData);
			Experience = new PcExperience(saveData, formula);

			Experience.OnLevelUp += LevelUp;
		}

		public void ExitTree()
		{
			Experience.OnLevelUp -= LevelUp;
		}
		
		public void CalculateStatModifiers(Dictionary<StatType, int> equipmentBonuses)
		{
			foreach (Stat stat in this)
			{
				stat.ClearModifiers();
				foreach (KeyValuePair<StatType, int> kvp in equipmentBonuses.Where(kvp => kvp.Key == stat.Type))
				{
					stat.AddModifier(kvp.Value);
				}
			}
		}
		
		public IEnumerator<Stat> GetEnumerator()
		{
			yield return _attack;
			yield return _defense;
			yield return _engineering;
			yield return _farming;
			yield return _medical;
			yield return _scavenging;
		}
	
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		public Stat GetStatByType(StatType type)
		{
			foreach (Stat stat in this)
			{
				if (stat.Type == type)
				{
					return stat;
				}
			}
			GD.PushWarning($"No stat found for type {type}");
			return null;
		}
		
		// Pain == 0 -> No effect. Pain == 100 -> Stat value halved. (for now at least)
		private int PainFormula(int statValue)
		{
		    return Mathf.RoundToInt(statValue * (200f - Health.Pain) / 200f);
		}

		private void LevelUp()
		{
			// TODO: Make level up menu, launch it from here somehow?
			// Or just have the ability to add skill points whenever there's point to spend.
			// Just use TeamData (while at home) to get PC's stats and hook it up to the CharacterMenu.
			StatPointsToSpend += STAT_POINTS_GAINED_PER_LEVEL;
			//this.PrintDebug($"Level up to {Experience.Level}, Experience: {Experience.Experience}");
		}
	}
}
