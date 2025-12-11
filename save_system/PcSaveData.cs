using System.Text.Json.Serialization;
using Godot;

namespace Lastdew
{
	public class PcSaveData
	{
		public long PcDataUid { get; }

		// Equipment
		public long Head { get; }
		public long Weapon { get; }
		public long Body { get; }
		public long Feet { get; }

		// Stats
		public int Injury { get; }
		public int Experience { get; }
		public int Attack { get; }
		public int Defense { get; }
		public int Engineering { get; }
		public int Farming { get; }
		public int Medical { get; }
		public int Scavenging { get; }

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

		[JsonConstructor]
		public PcSaveData(
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
			PcDataUid = pcDataUid;
			Head = head;
			Weapon = weapon;
			Body = body;
			Feet = feet;
			Injury = injury;
			Experience = experience;
			Attack = attack;
			Defense = defense;
			Engineering = engineering;
			Farming = farming;
			Medical = medical;
			Scavenging = scavenging;
		}

		public void PrintData()
		{
			//PcDataUid.PrintData();
			GD.Print($"Head Equipment: {Head}\n");
			GD.Print($"Attack: {Attack}\n");
		}
	}
}
