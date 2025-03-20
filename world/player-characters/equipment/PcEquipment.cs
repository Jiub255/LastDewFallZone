using Godot;
using System.Collections;
using System.Collections.Generic;

namespace Lastdew
{
	public class PcEquipment : IEnumerable<Equipment>
	{
		public Equipment Head { get; set; }
		public Equipment Weapon { get; set; }
		public Equipment Body { get; set; }
		public Equipment Feet { get; set; }
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
		
		// JUST FOR TESTING
		// TODO: How to store/access this in the future? Autoload? Pass it from the top down?
		private Craftables Craftables { get; }
		
		public PcEquipment(PcSaveData pcSaveData)
		{
			Craftables = ResourceLoader.Load<Craftables>(UIDs.CRAFTABLES);
			if (Craftables.Equipment.ContainsKey(pcSaveData.Head))
			{
				Equip((Equipment)Craftables[pcSaveData.Head]);
			}
			if (Craftables.Equipment.ContainsKey(pcSaveData.Weapon))
			{
				Equip((Equipment)Craftables[pcSaveData.Weapon]);
			}
			if (Craftables.Equipment.ContainsKey(pcSaveData.Body))
			{
				Equip((Equipment)Craftables[pcSaveData.Body]);
			}
			if (Craftables.Equipment.ContainsKey(pcSaveData.Feet))
			{
				Equip((Equipment)Craftables[pcSaveData.Feet]);
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
