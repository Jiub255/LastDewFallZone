namespace Lastdew
{
	public class PcSaveData
	{
		// TODO: How to id pc? Use index from allPcs? Or just use a name?
		// Figure out later after character models have been chosen, just load the test pcs for now.
		// Or maybe just replace string Name with PcData (which has name, icon, meshes, etc.)
		public string Name { get; set; }
		
		// Equipment
		public long Head { get; set; }
		public long Weapon { get; set; }
		public long Body { get; set; }
		public long Feet { get; set; }
		
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
			long head,
			long weapon,
			long body,
			long feet,
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
			Head = pc.Equipment.Head?.GetUid() ?? 0;
			Weapon = pc.Equipment.Weapon?.GetUid() ?? 0;
			Body = pc.Equipment.Body?.GetUid() ?? 0;
            Feet = pc.Equipment.Feet?.GetUid() ?? 0;
			Injury = pc.Health.Injury;
		}
	}
}
