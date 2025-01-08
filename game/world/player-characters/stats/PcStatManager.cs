
namespace Lastdew
{	public class PcStatManager
	{
		public PcStatsData Stats { get; set; }
		
		public PcStatManager(PcStatsData stats)
		{
			Stats = stats;
		}
		
		public void CalculateStatModifiers(StatAmount[] equipmentBonuses)
		{
			foreach (Stat stat in Stats)
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
			
			// TODO: Invoke event? Have Stat invoke events?
		}
		
		public bool MeetsRequirements(Equipment equipment)
		{
			bool requirementsMet = true;
			foreach (StatAmount statAmount in equipment.StatRequirements)
			{
				if (Stats.GetStatByType(statAmount.Type).Value < statAmount.Amount)
				{
					requirementsMet = false;
				}
			}
			return requirementsMet;
		}
	}
}
