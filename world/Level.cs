using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class Level : Node3D
	{
		[Export]
		public AudioStreamMP3 Song { get; private set; }

		public int PcsInExitZone => EntranceExit.PcCount;
		private EntranceExit EntranceExit { get; set; }
		
		/// <summary>
		/// Must be called after Level.Ready()
		/// </summary>
		public Vector3[] Initialize()
		{
			EntranceExit = GetNode<EntranceExit>("%EntranceExit");
			return EntranceExit.SpawnPoints;
		}
	}
}
