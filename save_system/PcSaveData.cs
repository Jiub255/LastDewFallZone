namespace Lastdew
{
	public class PcSaveData
	{
		// TODO: How to id pc? Use index from allPcs? Or just use a name?
		// Figure out later after character models have been chosen, just load the test pcs for now.
		// Or maybe just replace string Name with PcData (which has name, icon, meshes, etc.)
		public string Name { get; set; }
		
		// Equipment
		public string Head { get; set; }
		public string Weapon { get; set; }
		public string Body { get; set; }
		public string Feet { get; set; }
		
		// Stats
		public int Injury { get; set; }
		
		/// <summary>
		/// TODO: Make this the default starter character for a new game. 
		/// </summary>
		public PcSaveData()
		{
			// TODO: Just the default for testing. Eventually will change.
			Name = "James";
		}
		
		/// <summary>
		/// TODO: Change constructor parameters to match the properties of this class.
		/// Like in SaveData. Might be the problem.
		/// </summary>
		public PcSaveData(
			string name,
			string head,
			string weapon,
			string body,
			string feet,
			int injury)
		{
			Name = name;
			Head = head;
			Weapon = weapon;
			Body = body;
			Feet = feet;
			Injury = injury;
		}
		
		public PcSaveData(PlayerCharacter pc)
		{
			Name = pc.Name;
			Head = pc.Equipment.Head?.Name;
			Weapon = pc.Equipment.Weapon?.Name;
			Body = pc.Equipment.Body?.Name;
			Feet = pc.Equipment.Feet?.Name;
			Injury = pc.Health.Injury;
		}
	}
}
