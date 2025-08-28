using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class Level : Node3D
	{
		private Node3D SpawnLocation { get; set; }
		
		/// <returns>Spawn location</returns>
		public virtual Vector3 Initialize(TeamData teamData)
		{
			SpawnLocation = GetNode<Node3D>("%SpawnLocation");
			return SpawnLocation.Position;
		}
	}
}
