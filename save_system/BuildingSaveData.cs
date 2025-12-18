using System.Text.Json.Serialization;
using Godot;

namespace Lastdew
{
	[method: JsonConstructor]
	public class BuildingSaveData(long buildingUid, (float, float, float) position, float rotation)
	{
		public long BuildingUid { get; } = buildingUid;
		public (float, float, float) Position { get; } = position;
		public float Rotation { get; } = rotation;

		public BuildingSaveData(long buildingUid, Transform3D transform)
			: this(buildingUid, (transform.Origin.X, transform.Origin.Y, transform.Origin.Z), transform.Basis.GetEuler().Y)
		{
		}

		public void PrintData()
		{
			GD.Print($"UID: {BuildingUid},  Position: {Position},   Rotation: {Rotation}");
		}
	}
}
