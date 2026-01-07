using Godot;

namespace Lastdew
{
	[GlobalClass, Tool]
	public partial class Weapon : Equipment
	{
		public override EquipmentType EquipmentType
		{
			get => EquipmentType.WEAPON;
			protected set { }
		}
		// Used to find which animation to use for attack.
		[Export] public WeaponType WeaponType { get; private set; } = WeaponType.MELEE;
		[Export] public float Range { get; private set; } = 1f;
		[Export] public float TimeBetweenAttacks { get; private set; } = 1f;
		[Export] public AudioStream AttackSound { get; private set; }
	}
}
