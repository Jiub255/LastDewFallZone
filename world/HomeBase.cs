using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class HomeBase : Level
	{
		// 86400 seconds in a day => 2 * pi radians
		private const float LIGHT_ROTATION_SPEED = Mathf.Pi / 43200;
		
		public NavigationRegion3D NavMesh { get; private set; }
		private Node3D Buildings { get; set; }
		private DirectionalLight3D Light { get; set; }

		public override void Initialize(List<BuildingData> buildingDatas)
		{
			base.Initialize(buildingDatas);
			
			Buildings = GetNode<Node3D>("%Buildings");
			NavMesh = GetNode<NavigationRegion3D>("%NavigationRegion3D");
			Light = GetNode<DirectionalLight3D>("%DirectionalLight3D");
			
			PlaceBuildings(buildingDatas);
		}

		public void AddBuilding(Building3D building)
		{
			Buildings.AddChildDeferred(building);
		}

		public void RotateLight(float timeInSeconds)
		{
			float x = (timeInSeconds * LIGHT_ROTATION_SPEED) + (Mathf.Pi / 2);
			Light.Rotation = new Vector3(x, 0f, 0f);
		}
		
		private void PlaceBuildings(List<BuildingData> buildingDatas)
		{
			foreach (BuildingData data in buildingDatas)
			{
				Building building = Databases.Craftables.Buildings[data.BuildingUid];
				PackedScene buildingScene = GD.Load<PackedScene>(building.SceneUid);
				Building3D building3D = (Building3D)buildingScene.Instantiate();
				AddBuilding(building3D);
				building3D.Position = new Vector3(data.Position.X, data.Position.Y, data.Position.Z);
				building3D.Rotation = new Vector3(0f, data.Rotation, 0f);
				building3D.CallDeferred(Building3D.MethodName.SetBuilding);
			}
			
			NavMesh.CallDeferred(NavigationRegion3D.MethodName.BakeNavigationMesh);
		}
	}
}
