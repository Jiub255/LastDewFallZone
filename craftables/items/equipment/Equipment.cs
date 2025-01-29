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
		public StatAmount[] EquipmentBonuses { get; set; } = Array.Empty<StatAmount>();
		[Export]
		public StatAmount[] StatRequirementsToUse { get; set; } = Array.Empty<StatAmount>();
		
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
