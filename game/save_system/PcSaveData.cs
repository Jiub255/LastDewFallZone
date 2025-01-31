namespace Lastdew
{
	public class PcSaveData
	{
		// TODO: How to id pc? Use index from allPcs? Or just use a name?
		// Figure out later after character models have been chosen, just load the test pcs for now.
		public string Name { get; }
		
		// Equipment
		public string Head { get; }
		public string Weapon { get; }
		public string Body { get; }
		public string Feet { get; }
		
		// Stats
		public int Injury { get; set; }
		
		public PcSaveData()
		{
			// TODO: Just the default for testing. Eventually will change.
			Name = "James";
		}
		
		public PcSaveData(string name, PcEquipment equipment, PcHealth health)
		{
			Name = name;
			
			Head = equipment.Head?.Name;
			Weapon = equipment.Weapon?.Name;
			Body = equipment.Body?.Name;
			Feet = equipment.Feet?.Name;
			
			Injury = health.Injury;
		}
	}
}
