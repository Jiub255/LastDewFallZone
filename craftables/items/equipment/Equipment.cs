using Godot;
using Godot.Collections;
using System;

namespace Lastdew
{
	[GlobalClass, Tool]
	public partial class Equipment : Item
	{
		[Export] public virtual EquipmentType EquipmentType { get; protected set; } = EquipmentType.HEAD;

		[Export] public Dictionary<StatType, int> EquipmentBonuses { get; private set; } = [];

		[Export] public Dictionary<StatType, int> StatsNeededToEquip { get; private set; } = [];

		// TODO: Add this to the resource editor as a long UID.
		[Export] public string SceneUid { get; private set; }

		
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
