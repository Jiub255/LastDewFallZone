using Godot;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lastdew
{
	public class PcStatManager : IEnumerable<Stat>
	{
		private Stat _attack = new(StatType.ATTACK, 1);
		private Stat _defense = new(StatType.DEFENSE, 1);
		private Stat _engineering = new(StatType.ENGINEERING, 1);
		private Stat _farming = new(StatType.FARMING, 1);
		private Stat _medical = new(StatType.MEDICAL, 1);
		private Stat _scavenging = new(StatType.SCAVENGING, 1);
		private int _pain;
		
		public int Attack
		{
			get => PainFormula(_attack.Value, _pain);
		}
		public int Defense
		{
			get => PainFormula(_defense.Value, _pain);
		}
		public int Engineering
		{
			get => PainFormula(_engineering.Value, _pain);
		}
		public int Farming
		{
			get => PainFormula(_farming.Value, _pain);
		}
		public int Medical
		{
			get => PainFormula(_medical.Value, _pain);
		}
		public int Scavenging
		{
			get => PainFormula(_scavenging.Value, _pain);
		}

		public void SetPain(int pain)
		{
			_pain = pain;
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
		
		public bool MeetsRequirements(Equipment equipment)
		{
			bool requirementsMet = true;
			foreach (KeyValuePair<StatType, int> kvp in equipment.StatsNeededToEquip
				         .Where(kvp => GetStatByType(kvp.Key)?.Value < kvp.Value))
			{
				requirementsMet = false;
			}
			return requirementsMet;
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
		
		private Stat GetStatByType(StatType type)
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
		private static int PainFormula(int statValue, int pain)
		{
		    return Mathf.RoundToInt(statValue * (200f - pain) / 200f);
		}
	}
}
