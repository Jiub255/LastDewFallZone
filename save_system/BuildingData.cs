using Godot;

namespace Lastdew
{
	public partial class BuildingData : RefCounted
	{
		public long BuildingUid { get; }
		public Vector3 Position { get; }
		public float Rotation { get; }

		public BuildingData(long buildingUid, Transform3D transform)
			: this(buildingUid, new Vector3(transform.Origin.X, transform.Origin.Y, transform.Origin.Z), transform.Basis.GetEuler().Y)
		{
		}

		public BuildingData(BuildingSaveData data) : this(data.BuildingUid, new Vector3(data.X, data.Y, data.Z), data.Rotation)
		{
		}

		public BuildingData()
		{
		}

		public BuildingData(long buildingUid, Vector3 position, float rotation)
		{
			BuildingUid = buildingUid;
			Position = position;
			Rotation = rotation;
		}
	}
}
