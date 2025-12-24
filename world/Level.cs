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
		
		public virtual void Initialize(List<BuildingData> buildingDatas)
		{
			SpawnLocation = GetNode<Node3D>("%SpawnLocation");
		}
	}
}
