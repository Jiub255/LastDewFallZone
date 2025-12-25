using System;

namespace Lastdew
{
	public class MoraleManager
	{
		private const int MIN_MORALE = -100;
		private const int MAX_MORALE = 100;
		
		private int _morale;
		private TeamData _teamData;

		public int Morale
		{
			get => _morale;
			// TODO: On set, change all the things that morale affects. Stats, odds of new survivors, etc.
			// Might do directly through TeamData or just invoke an event.
			set => _morale = Math.Clamp(value, MIN_MORALE, MAX_MORALE);
		}
		
		/// <summary>
		/// Have to wait until after PCs are spawned (in PcManager) to instantiate this,
		/// since TeamData.Pcs will be empty until then.
		/// Actually just have a separate Setup() method for dealing with that.
		/// </summary>
		public MoraleManager(TeamData teamData)
		{
			_teamData = teamData;
			// TODO: Subscribe to all the "changed" events from food, water, buildings,
			// anything that affects morale.
		}

		// TODO: Subscribe to all PC related events. Calculate morale based on all its factors,
		// then apply that to pcs stats.
		public void Setup()
		{
			
		}
	}
}
