using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	/// <summary>
	/// TODO: Redo this whole situation using one PC packed scene, and the mesh indexes.
	/// Maybe use PcData class again or something.
	/// </summary>
	[GlobalClass]
	public partial class AllPcScenes : Resource
	{
		public Dictionary<string, PackedScene> PcScenes { get; set; } = new()
		{
			{"James", GD.Load<PackedScene>(UIDs.TEST_PC_1)},
			{"Jaime", GD.Load<PackedScene>(UIDs.TEST_PC_2) },
		};

        public PackedScene this[string name] => PcScenes[name];
    }
}
