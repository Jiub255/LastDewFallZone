using System.Text.Json.Serialization;
using Godot;

namespace Lastdew
{
	[method: JsonConstructor]
	public class PcSaveData(
		long pcDataUid = 0,
		long head = 0,
		long weapon = 0,
		long body = 0,
		long feet = 0,
		int injury = 0,
		int experience = 0,
		int attack = 1,
		int defense = 1,
		int engineering = 1,
		int farming = 1,
		int medical = 1,
		int scavenging = 1)
	{
		public long PcDataUid { get; } = pcDataUid;

		// Equipment
		public long Head { get; } = head;
		public long Weapon { get; } = weapon;
		public long Body { get; } = body;
		public long Feet { get; } = feet;

		// Stats
		public int Injury { get; } = injury;
		public int Experience { get; } = experience;
		public int Attack { get; } = attack;
		public int Defense { get; } = defense;
		public int Engineering { get; } = engineering;
		public int Farming { get; } = farming;
		public int Medical { get; } = medical;
		public int Scavenging { get; } = scavenging;

		public PcSaveData(PlayerCharacter pc) : this(
			pc.Data.GetUid(),
			pc.Equipment.Head?.GetUid() ?? 0,
			pc.Equipment.Weapon?.GetUid() ?? 0,
			pc.Equipment.Body?.GetUid() ?? 0,
			pc.Equipment.Feet?.GetUid() ?? 0,
			pc.StatManager.Health.Injury,
			pc.StatManager.Experience.Experience,
			pc.StatManager.AttackBaseValue,
			pc.StatManager.DefenseBaseValue,
			pc.StatManager.EngineeringBaseValue,
			pc.StatManager.FarmingBaseValue,
			pc.StatManager.MedicalBaseValue,
			pc.StatManager.ScavengingBaseValue) {}

		public PcSaveData(PcData data) : this(
			data.GetUid(),
			0,
			0,
			0,
			0,
			0,
			0,
			data.Attack,
			data.Defense,
			data.Engineering,
			data.Farming,
			data.Medical,
			data.Scavenging
			) {}

		public void PrintData()
		{
			//PcDataUid.PrintData();
			GD.Print($"Head Equipment: {Head}\n");
			GD.Print($"Attack: {Attack}\n");
		}
	}
}
