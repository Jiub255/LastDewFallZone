using System.Collections;
using System.Collections.Generic;

namespace Lastdew
{
	public class PcEquipment : IEnumerable<Equipment>
	{
		public Equipment Head { get; set; }
		public Equipment Body { get; set; }
		public Equipment Feet { get; set; }
		public Equipment Weapon { get; set; }
		public StatAmount[] Bonuses
		{
			get
			{
				List<StatAmount> equipmentBonuses = new();
				foreach (Equipment equipment in this)
				{
					equipmentBonuses.AddRange(equipment.EquipmentBonuses);
				}
				return equipmentBonuses.ToArray();
			}
		}
		
		public PcEquipment() {}
		
		/// <returns>Whatever was equipped before (null if nothing)</returns>
		public Equipment Equip(Equipment equipment)
		{
			Equipment previous = null;
			switch (equipment.Type)
			{
				case EquipmentType.HEAD:
					previous = Head;
					Head = equipment;
					break;
				case EquipmentType.BODY:
					previous = Body;
					Body = equipment;
					break;
				case EquipmentType.FEET:
					previous = Feet;
					Feet = equipment;
					break;
				case EquipmentType.WEAPON:
					previous = Weapon;
					Weapon = equipment;
					break;
			}
			return previous;
		}
		
		/// <returns>Whatever was equipped before (null if nothing)</returns>
		public Equipment Unequip(EquipmentType equipmentType)
		{
			Equipment previous = null;
			switch (equipmentType)
			{
				case EquipmentType.HEAD:
					previous = Head;
					Head = null;
					break;
				case EquipmentType.BODY:
					previous = Body;
					Body = null;
					break;
				case EquipmentType.FEET:
					previous = Feet;
					Feet = null;
					break;
				case EquipmentType.WEAPON:
					previous = Weapon;
					Weapon = null;
					break;
			}
			return previous;
		}

		public IEnumerator<Equipment> GetEnumerator()
		{
			yield return Head;
			yield return Body;
			yield return Feet;
			yield return Weapon;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
