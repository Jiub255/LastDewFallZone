using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class Level : Node3D
	{
		[Export]
		public AudioStreamMP3 Song { get; private set; }
		public EntranceExit EntranceExit { get; private set; }

		public override void _Ready()
		{
			EntranceExit = GetNode<EntranceExit>("%EntranceExit");
		}
	}
}
