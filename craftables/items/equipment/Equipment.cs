using Godot;
using Godot.Collections;
using System;

namespace Lastdew
{
	[GlobalClass, Tool]
	public partial class Equipment : Item
	{
		[Export]
		public EquipmentType Type { get; private set; } = EquipmentType.HEAD;

		[Export]
		public Dictionary<StatType, int> EquipmentBonuses { get; private set; } = [];

		[Export]
		public Dictionary<StatType, int> StatsNeededToEquip { get; private set; } = [];

		public override void OnClickCraftable()
		{
			throw new NotImplementedException();
		}
	
		public override void OnClickItem(PlayerCharacter pc)
		{
			pc.Equip(this);
		}
	}
}
