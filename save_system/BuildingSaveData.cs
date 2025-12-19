using System.Text.Json.Serialization;

namespace Lastdew
{
	public class BuildingSaveData
	{
		public long BuildingUid { get; }
		public float X { get; }
		public float Y { get; }
		public float Z { get; }
		public float Rotation { get; }
		
		public BuildingSaveData(){}

		public BuildingSaveData(BuildingData data)
		{
			BuildingUid = data.BuildingUid;
			X = data.Position.X;
			Y = data.Position.Y;
			Z = data.Position.Z;
			Rotation = data.Rotation;
		}

		[method: JsonConstructor]
		public BuildingSaveData(long buildingUid,
			float x, float y, float z, float rotation)
		{
			BuildingUid = buildingUid;
			X = x;
			Y = y;
			Z = z;
			Rotation = rotation;
		}
	}
}
