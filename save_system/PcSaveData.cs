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
			Head = pc.Equipment.Head == null ? 0 : pc.Equipment.Head.GetUid();
			Weapon = pc.Equipment.Weapon == null ? 0 : pc.Equipment.Weapon.GetUid();
			Body = pc.Equipment.Body == null ? 0 : pc.Equipment.Body.GetUid();
            Feet = pc.Equipment.Feet == null ? 0 : pc.Equipment.Feet.GetUid();
			Injury = pc.Health.Injury;
		}
	}
}
