using Godot;
using System;
using System.Linq;

namespace Lastdew
{
	[GlobalClass, Tool]
	public partial class Equipment : Item
	{
		private EquipmentType _equipmentType = EquipmentType.HEAD;

		[Export]
		public virtual EquipmentType EquipmentType
		{
			get => _equipmentType;
			protected set
			{
				if (value == EquipmentType.WEAPON)
				{
					GD.PushWarning($"Weapons must be Weapon type, not Equipment.");
					return;
				}
				_equipmentType = value;
			}
		}

		[Export] public Godot.Collections.Dictionary<StatType, int> EquipmentBonuses { get; private set; } = [];

		[Export] public Godot.Collections.Dictionary<StatType, int> StatsNeededToEquip { get; private set; } = [];

		// TODO: Add this to the resource editor as a long UID.
		[Export] public string SceneUid { get; private set; }

		
		public override void OnClickItem(TeamData teamData)
		{
			PlayerCharacter pc = teamData.Pcs[teamData.MenuSelectedIndex];
			pc.Equip(this);
		}

		public bool HasStatsToEquip(PcStatManager stats)
		{
			return StatsNeededToEquip
				.All(kvp => stats.GetStatByType(kvp.Key).Value >= kvp.Value);
		}
	}
}
