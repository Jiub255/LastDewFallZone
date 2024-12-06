using System.Collections;
using System.Collections.Generic;

namespace Lastdew
{	
	public class PcStatsData : IEnumerable<Stat>
	{
		public Stat Attack;
		public Stat Defense;
		public Stat Engineering;
		public Stat Farming;
		public Stat Medical;
		public Stat Scavenging;
	
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
