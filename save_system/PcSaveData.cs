namespace Lastdew
{
	public class PcSaveData(
		PcData data,
		long head = 0,
		long weapon = 0,
		long body = 0,
		long feet = 0,
		int injury = 0)
	{
		public PcData PcData { get; set; } = data;

		// Equipment
		public long Head { get; set; } = head;
		public long Weapon { get; set; } = weapon;
		public long Body { get; set; } = body;
		public long Feet { get; set; } = feet;

		// Stats
		public int Injury { get; set; } = injury;

		public PcSaveData(PlayerCharacter pc) : this(
			pc.Data,
			pc.Equipment.Head?.GetUid() ?? 0,
			pc.Equipment.Weapon?.GetUid() ?? 0,
			pc.Equipment.Body?.GetUid() ?? 0,
			pc.Equipment.Feet?.GetUid() ?? 0,
			pc.Health.Injury) {}
	}
}
