using Godot;
using System.Collections;
using System.Collections.Generic;

namespace Lastdew
{
	public class PcStatManager : IEnumerable<Stat>
	{
		private Stat _attack;
		private Stat _defense;
		private Stat _engineering;
		private Stat _farming;
		private Stat _medical;
		private Stat _scavenging;
		private int _pain;
		
		// Pain == 0 -> No effect. Pain == 100 -> Stat value halved. (for now at least)
		public int Attack { get => _attack.Value * (200 - _pain) / 200; }
		public int Defense { get => _defense.Value * (200 - _pain) / 200; }
		public int Engineering { get => _engineering.Value * (200 - _pain) / 200; }
		public int Farming { get => _farming.Value * (200 - _pain) / 200; }
		public int Medical { get => _medical.Value * (200 - _pain) / 200; }
		public int Scavenging { get => _scavenging.Value * (200 - _pain) / 200; }
		
		public PcStatManager()
		{
			_attack = new Stat(StatType.ATTACK, 1);
			_defense = new Stat(StatType.DEFENSE, 1);
			// SET TO 100 FOR TESTING
			_engineering = new Stat(StatType.ENGINEERING, 100);
			_farming = new Stat(StatType.FARMING, 1);
			_medical = new Stat(StatType.MEDICAL, 1);
			_scavenging = new Stat(StatType.SCAVENGING, 1);
		}
		
		public void SetPain(int pain)
		{
			_pain = pain;
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
			foreach (StatAmount statAmount in equipment.StatRequirementsToUse)
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
	}
}
