using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class Level : Node3D
	{
		private Node3D SpawnLocation { get; set; }
		
		public virtual void Initialize(TeamData teamData)
		{
			SpawnLocation = GetNode<Node3D>("%SpawnLocation");
		}
	}
}
