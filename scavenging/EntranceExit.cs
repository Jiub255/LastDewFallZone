using Godot;

namespace Lastdew
{
	public partial class EntranceExit : Node3D
	{
		private const int MAX_TEAM_SIZE = 6;

		public Vector3[] SpawnPoints
		{
			get
			{
				Vector3[] spawnPoints = new Vector3[MAX_TEAM_SIZE];
				int index = 0;
				foreach (Node child in GetChildren())
				{
					if (child is not Marker3D marker) continue;
					if (index >= MAX_TEAM_SIZE)
					{
						GD.PushError($"More spawn points than max team size ({MAX_TEAM_SIZE})");
						return spawnPoints;
					}
					spawnPoints[index] = marker.GlobalPosition;
					index++;
				}
				return spawnPoints;
			}
		}
		public int PcCount { get; private set; }
		private Area3D PcDetector { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			PcDetector = GetNode<Area3D>("%PcDetector");
			PcDetector.BodyEntered += IncrementPcCount;
			PcDetector.BodyExited += DecrementPcCount;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			PcDetector.BodyEntered -= IncrementPcCount;
			PcDetector.BodyExited -= DecrementPcCount;
		}

		public void IncrementPcCount()
		{
			PcCount++;
		}
		
		private void IncrementPcCount(Node3D body)
		{
			if (body is PlayerCharacter)
			{
				PcCount++;
			}
		}

		private void DecrementPcCount(Node3D body)
		{
			if (body is PlayerCharacter)
			{
				PcCount--;
			}
		}
	}
}
