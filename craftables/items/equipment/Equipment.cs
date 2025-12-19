using Godot;
using System;
using System.Linq;

namespace Lastdew
{
	[GlobalClass, Tool]
	public partial class Equipment : Item
	{
		[Export] public virtual EquipmentType EquipmentType { get; protected set; } = EquipmentType.HEAD;

		[Export] public Godot.Collections.Dictionary<StatType, int> EquipmentBonuses { get; private set; } = [];

		[Export] public Godot.Collections.Dictionary<StatType, int> StatsNeededToEquip { get; private set; } = [];

		// TODO: Add this to the resource editor as a long UID.
		[Export] public string SceneUid { get; private set; }

		
		public override void OnClickItem(PlayerCharacter pc)
		{
			pc.Equip(this);
		}

		public bool HasStatsToEquip(PcStatManager stats)
		{
			return StatsNeededToEquip
				.All((kvp) => stats.GetStatByType(kvp.Key).Value >= kvp.Value);
		}
	}
}
