using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class AllPcScenes : Resource
	{
		public Dictionary<string, PackedScene> PcScenes { get; set; } = new()
		{
			{"James", GD.Load<PackedScene>("res://world/player-characters/humans_master.tscn")},
			{"Jaime", GD.Load<PackedScene>("res://world/player-characters/humans_master2.tscn") },
		};

        public PackedScene this[string name] => PcScenes[name];
    }
}
