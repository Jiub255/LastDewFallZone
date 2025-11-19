namespace Lastdew
{
	public class PcSaveData(
		PcData data,
		long head = 0,
		long weapon = 0,
		long body = 0,
		long feet = 0,
		int injury = 0,
		int attack = 0,
		int defense = 0,
		int engineering = 0,
		int farming = 0,
		int medical = 0,
		int scavenging = 0)
	{
		public PcData PcData { get; } = data;

		// Equipment
		public long Head { get; } = head;
		public long Weapon { get; } = weapon;
		public long Body { get; } = body;
		public long Feet { get; } = feet;

		// Stats
		public int Injury { get; } = injury;
		public int Attack { get; } = attack;
		public int Defense { get; } = defense;
		public int Engineering { get; } = engineering;
		public int Farming { get; } = farming;
		public int Medical { get; } = medical;
		public int Scavenging { get; } = scavenging;

		public PcSaveData(PlayerCharacter pc) : this(
			pc.Data,
			pc.Equipment.Head?.GetUid() ?? 0,
			pc.Equipment.Weapon?.GetUid() ?? 0,
			pc.Equipment.Body?.GetUid() ?? 0,
			pc.Equipment.Feet?.GetUid() ?? 0,
			pc.StatManager.Health.Injury,
			pc.StatManager.AttackBaseValue,
			pc.StatManager.DefenseBaseValue,
			pc.StatManager.EngineeringBaseValue,
			pc.StatManager.FarmingBaseValue,
			pc.StatManager.MedicalBaseValue,
			pc.StatManager.ScavengingBaseValue) {}
	}
}
