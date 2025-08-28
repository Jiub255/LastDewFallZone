using Godot;
using System.Collections;
using System.Collections.Generic;

namespace Lastdew
{
	public class PcEquipment : IEnumerable<Equipment>
	{
		public Equipment Head { get; private set; }
		public Equipment Weapon { get; private set; }
		public Equipment Body { get; private set; }
		public Equipment Feet { get; private set; }
		public Dictionary<StatType, int> Bonuses
		{
			get
			{
				Dictionary<StatType, int> equipmentBonuses = new();
				foreach (Equipment equipment in this)
				{
					if (equipment != null && equipment.EquipmentBonuses.Count > 0)
					{
						foreach (KeyValuePair<StatType, int> kvp in equipment.EquipmentBonuses)
						{
						    if (equipmentBonuses.ContainsKey(kvp.Key))
						    {
						        equipmentBonuses[kvp.Key] += kvp.Value;
						    }
						    else
						    {
						        equipmentBonuses[kvp.Key] = kvp.Value;
						    }
						}
					}
				}
				return equipmentBonuses;
			}
		}
		
		public PcEquipment(PcSaveData pcSaveData)
		{
			Craftables craftables = Databases.CRAFTABLES;
			if (craftables.Equipments.ContainsKey(pcSaveData.Head))
			{
				Equip((Equipment)craftables[pcSaveData.Head]);
			}
			if (craftables.Equipments.ContainsKey(pcSaveData.Weapon))
			{
				Equip((Equipment)craftables[pcSaveData.Weapon]);
			}
			if (craftables.Equipments.ContainsKey(pcSaveData.Body))
			{
				Equip((Equipment)craftables[pcSaveData.Body]);
			}
			if (craftables.Equipments.ContainsKey(pcSaveData.Feet))
			{
				Equip((Equipment)craftables[pcSaveData.Feet]);
			}
		}
		
		/// <returns>Whatever was equipped before (null if nothing)</returns>
		public Equipment Equip(Equipment equipment)
		{
			Equipment previous;
			switch (equipment.Type)
			{
				case EquipmentType.HEAD:
					previous = Head;
					Head = equipment;
					break;
				case EquipmentType.WEAPON:
					previous = Weapon;
					Weapon = equipment;
					break;
				case EquipmentType.BODY:
					previous = Body;
					Body = equipment;
					break;
				case EquipmentType.FEET:
					previous = Feet;
					Feet = equipment;
					break;
				default:
					previous = null;
					GD.PushWarning($"No equipment slot for type {equipment.Type}");
					break;
			}
			return previous;
		}
		
		/// <returns>Whatever was equipped before (null if nothing)</returns>
		public Equipment Unequip(EquipmentType equipmentType)
		{
			Equipment previous;
			switch (equipmentType)
			{
				case EquipmentType.HEAD:
					previous = Head;
					Head = null;
					break;
				case EquipmentType.WEAPON:
					previous = Weapon;
					Weapon = null;
					break;
				case EquipmentType.BODY:
					previous = Body;
					Body = null;
					break;
				case EquipmentType.FEET:
					previous = Feet;
					Feet = null;
					break;
				default:
					previous = null;
					GD.PushWarning($"No equipment slot for type {equipmentType}");
					break;
			}
			return previous;
		}

		public IEnumerator<Equipment> GetEnumerator()
		{
			yield return Head;
			yield return Weapon;
			yield return Body;
			yield return Feet;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
