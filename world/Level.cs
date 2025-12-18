using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class Level : Node3D
	{
		[Export]
		public AudioStreamMP3 Song { get; private set; }
		private Node3D SpawnLocation { get; set; }
		
		// TODO: Separate these into subclass Homebase : Level?
		public NavigationRegion3D NavMesh { get; private set; }
		private Node3D Buildings { get; set; }
		
		public void Initialize()
		{
			SpawnLocation = GetNode<Node3D>("%SpawnLocation");
			Buildings = GetNode<Node3D>("%Buildings");
			NavMesh = GetNode<NavigationRegion3D>("%NavigationRegion3D");
		}

		public void AddBuilding(Node3D building)
		{
			Buildings.CallDeferred(Node.MethodName.AddChild, building);
		}
	}
}
