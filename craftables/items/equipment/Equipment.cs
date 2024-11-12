using Godot;
using System;

// TODO: Use enum or subclasses?
public enum EquipmentType
{
	HEAD,
	BODY,
	FEET,
	WEAPON,
}

[GlobalClass]
public partial class Equipment : Item
{
	[Export]
	public EquipmentType Type { get; set; }
	[Export]
	public StatAmount[] EquipmentBonuses { get; set; }
	[Export]
	public StatAmount[] StatRequirements { get; set; }
	
	public override void OnClickCraftable()
	{
		throw new NotImplementedException();
	}

	public override void OnClickItem()
	{
		throw new NotImplementedException();
	}
}
