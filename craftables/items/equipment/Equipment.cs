using Godot;
using System;

namespace Lastdew
{
	[GlobalClass, Tool]
	public partial class Equipment : Item
	{
		[Export]
		public EquipmentType Type { get; set; }
		[Export]
		public Godot.Collections.Dictionary<StatType, int> EquipmentBonuses { get; set; }
		[Export]
		public Godot.Collections.Dictionary<StatType, int>  StatsNeededToEquip { get; set; }
		
		public Equipment(){}
		
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
