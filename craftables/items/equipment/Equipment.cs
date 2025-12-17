using Godot;
using Godot.Collections;
using System;

namespace Lastdew
{
	[GlobalClass, Tool]
	public partial class Equipment : Item
	{
		[Export] public EquipmentType Type { get; private set; } = EquipmentType.HEAD;

		[Export] public Dictionary<StatType, int> EquipmentBonuses { get; private set; } = [];

		[Export] public Dictionary<StatType, int> StatsNeededToEquip { get; private set; } = [];

		#region AddTheseToWeaponClassMaybeNotSceneUID
		
		// TODO: Add this to the resource editor as a long UID.
		[Export] public string SceneUid { get; private set; }

		// TODO: Make a Weapon class that inherits Equipment and put WeaponType and Range there.
		// Used to find what animation to use for attack.
		[Export] public WeaponType WeaponType { get; private set; } = WeaponType.MELEE;
		[Export] public float Range { get; private set; } = 1f;

		#endregion
		
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
