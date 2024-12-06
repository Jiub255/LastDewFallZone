
namespace Lastdew
{	public class PcStatManager
	{
		public PcStatsData Stats { get; set; }
		
		public PcStatManager(PcStatsData stats)
		{
			Stats = stats;
		}
		
		// Connect with event from equipment manager?
		private void CalculateStatModifiers(StatAmount[] EquipmentBonuses)
		{
			foreach (Stat stat in Stats)
			{
				stat.ClearModifiers();
				foreach (StatAmount bonus in EquipmentBonuses)
				{
					if (bonus.Type == stat.Type)
					{
						stat.AddModifier(bonus.Amount);
					}
				}
			}
			
			// TODO: Invoke event?
		}
	}
}
