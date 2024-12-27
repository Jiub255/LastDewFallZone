using Godot;

namespace Lastdew
{
	public partial class AllPcScenes : Resource
	{
		public PackedScene[] PcScenes { get; set; } = new PackedScene[]
		{
			GD.Load<PackedScene>("res://game/world/player-characters/humans_master.tscn"),
			GD.Load<PackedScene>("res://game/world/player-characters/humans_master2.tscn"),
		};
	}
}
