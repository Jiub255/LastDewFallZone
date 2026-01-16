using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

namespace Lastdew
{
	public class Buildings : IEnumerable<BuildingData>
	{
		private readonly List<BuildingData> _buildingDatas = [];

		public int FoodProductionPerDay => Effects.GetValueOrDefault(BuildingEffect.FOOD_PRODUCTION, 0);
		public int WaterProductionPerDay => Effects.GetValueOrDefault(BuildingEffect.WATER_PRODUCTION, 0);
		public int MedicalSkillIncrease => Effects.GetValueOrDefault(BuildingEffect.MEDICAL_INCREASE, 0);
		public int MoraleIncrease => Effects.GetValueOrDefault(BuildingEffect.MORALE_INCREASE, 0);

		private ReadOnlyCollection<BuildingData> BuildingDatas => _buildingDatas.AsReadOnly();
		private Dictionary<BuildingEffect, int> Effects { get; } = [];

		public Buildings() {}

		public Buildings(List<BuildingData> buildings)
		{
			foreach (BuildingData building in buildings)
			{
				AddBuilding(building);
			}
		}
		
		public void AddBuilding(BuildingData data)
		{
			Building building = Databases.Craftables.Buildings[data.BuildingUid];
			
			foreach ((BuildingEffect type, int strength) in building.Effects)
			{
				if (!Effects.TryAdd(type, strength))
				{
					Effects[type] += strength;
				}
			}
			
			_buildingDatas.Add(data);
		}

		public void RemoveBuilding(BuildingData data)
		{
			Building building = Databases.Craftables.Buildings[data.BuildingUid];
			foreach ((BuildingEffect type, int strength) in building.Effects)
			{
				if (Effects.ContainsKey(type))
				{
					Effects[type] -= strength;
					if (Effects[type] <= 0)
					{
						Effects.Remove(type);
					}
				}
				else
				{
					GD.PushError($"Trying to remove effect type {type.ToString()} but it doesn't exist");
				}
			}
			
			_buildingDatas.Remove(data);
		}
		
		public IEnumerator<BuildingData> GetEnumerator()
		{
			return BuildingDatas.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
