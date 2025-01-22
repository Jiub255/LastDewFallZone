using Godot;
using System.Collections;
using System.Collections.Generic;

namespace Lastdew
{
	public class PcStatManager : IEnumerable<Stat>
	{
		public Stat Attack { get; set; }
		public Stat Defense { get; set; }
		public Stat Engineering { get; set; }
		public Stat Farming { get; set; }
		public Stat Medical { get; set; }
		public Stat Scavenging { get; set; }
		
		public PcStatManager()
		{
			Attack = new Stat(StatType.ATTACK, 1);
			Defense = new Stat(StatType.DEFENSE, 1);
			Engineering = new Stat(StatType.ENGINEERING, 1);
			Farming = new Stat(StatType.FARMING, 1);
			Medical = new Stat(StatType.MEDICAL, 1);
			Scavenging = new Stat(StatType.SCAVENGING, 1);
		}
		
		public void CalculateStatModifiers(StatAmount[] equipmentBonuses)
		{
			foreach (Stat stat in this)
			{
				stat.ClearModifiers();
				foreach (StatAmount bonus in equipmentBonuses)
				{
					if (bonus.Type == stat.Type)
					{
						stat.AddModifier(bonus.Amount);
					}
				}
			}
		}
		
		public bool MeetsRequirements(Equipment equipment)
		{
			bool requirementsMet = true;
			foreach (StatAmount statAmount in equipment.StatRequirements)
			{
				if (GetStatByType(statAmount.Type)?.Value < statAmount.Amount)
				{
					requirementsMet = false;
				}
			}
			return requirementsMet;
		}
	
		public IEnumerator<Stat> GetEnumerator()
		{
			yield return Attack;
			yield return Defense;
			yield return Engineering;
			yield return Farming;
			yield return Medical;
			yield return Scavenging;
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
	}
}
