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
		
		public void AddItems(Texture2D icon, string newAmountAndName)
		{
			if (ItemAmounts.TryGetValue(icon, out string amountAndName))
			{
				string[] newSeparated = newAmountAndName.Split(' ');
				int newAmount = int.Parse(newSeparated[0]);
				string name = newSeparated[1];
				
				string[] separated = amountAndName.Split(' ');
				int amount = int.Parse(separated[0]);
				ItemAmounts[icon] = $"{amount + newAmount} {name}";
			}
			else
			{
				ItemAmounts[icon] = newAmountAndName;
			}
		}
	}
}
