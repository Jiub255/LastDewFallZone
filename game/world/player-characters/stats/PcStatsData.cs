using System.Collections;
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public class PcStatsData : IEnumerable<Stat>
	{
		public Stat Attack { get; set; }
		public Stat Defense { get; set; }
		public Stat Engineering { get; set; }
		public Stat Farming { get; set; }
		public Stat Medical { get; set; }
		public Stat Scavenging { get; set; }
	
		// TODO: How to initialize?
		public PcStatsData()
		{
			Attack = new Stat(StatType.ATTACK, 1);
			Defense = new Stat(StatType.DEFENSE, 1);
			Engineering = new Stat(StatType.ENGINEERING, 1);
			Farming = new Stat(StatType.FARMING, 1);
			Medical = new Stat(StatType.MEDICAL, 1);
			Scavenging = new Stat(StatType.SCAVENGING, 1);
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
	}
}
