using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	[GlobalClass]
	public partial class AllPcScenes : Resource
	{
		public Dictionary<string, PackedScene> PcScenes { get; set; } = new()
		{
			{"James", GD.Load<PackedScene>(UIDs.PC_1)},
			{"Jaime", GD.Load<PackedScene>(UIDs.PC_2) },
		};

        public PackedScene this[string name] => PcScenes[name];
    }
}
