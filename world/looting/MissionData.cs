using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

namespace Lastdew
{
	/// <summary>
	/// Class to store items found, exp gained, injuries, etc. from the scavenging mission.
	/// </summary>
	public class MissionData
	{
		public Dictionary<Texture2D, string> ItemAmounts { get; } = [];
		public Dictionary<string, (int beginningExp, int beginningInjury)> InitialPcStats { get; } = [];

		
		public MissionData(ReadOnlyCollection<PlayerCharacter> pcs)
		{
			foreach (PlayerCharacter pc in pcs)
			{
				InitialPcStats[pc.Data.Name] = (
					pc.StatManager.Experience.Experience, 
					pc.StatManager.Health.Injury);
			}
		}
		
		public void AddItems(Texture2D icon, string amountAndName)
		{
			if (ItemAmounts.ContainsKey(icon))
			{
				ItemAmounts[icon] += amountAndName;
			}
			else
			{
				ItemAmounts[icon] = amountAndName;
			}
		}
	}
}
