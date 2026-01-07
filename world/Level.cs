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
		
		public Vector3 Initialize()
		{
			SpawnLocation = GetNode<Node3D>("%SpawnLocation");
			return SpawnLocation.Position;
		}
	}
}
