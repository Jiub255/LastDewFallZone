using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class Level : Node3D
	{
		[Export]
		public AudioStreamMP3 Song { get; private set; }
		private Node3D SpawnLocation { get; set; }
		
		// TODO: Separate these into subclass HomeBase : Level?
		public NavigationRegion3D NavMesh { get; private set; }
		private Node3D Buildings { get; set; }
		
		public void Initialize(List<BuildingSaveData> buildingDatas, bool scavengingLevel = false)
		{
			SpawnLocation = GetNode<Node3D>("%SpawnLocation");

			if (scavengingLevel)
			{
				return;
			}
			
			Buildings = GetNode<Node3D>("%Buildings");
			NavMesh = GetNode<NavigationRegion3D>("%NavigationRegion3D");
			PlaceBuildings(buildingDatas);
		}

		public void AddBuilding(Building3D building)
		{
			Buildings.CallDeferred(Node.MethodName.AddChild, building);
		}

		// TODO: Separate this into subclass HomeBase : Level?
		private void PlaceBuildings(List<BuildingSaveData> buildingDatas)
		{
			foreach (BuildingSaveData data in buildingDatas)
			{
				Building building = Databases.Craftables.Buildings[data.BuildingUid];
				PackedScene buildingScene = GD.Load<PackedScene>(building.SceneUid);
				Building3D building3D = (Building3D)buildingScene.Instantiate();
				AddBuilding(building3D);
				building3D.Position = new Vector3(data.Position.Item1, data.Position.Item2, data.Position.Item3);
				building3D.Rotation = new Vector3(0f, data.Rotation, 0f);
				building3D.CallDeferred(Building3D.MethodName.SetBuilding);
			}
			
			NavMesh.CallDeferred(NavigationRegion3D.MethodName.BakeNavigationMesh);
		}
	}
}
