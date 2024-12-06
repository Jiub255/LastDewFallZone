using Godot;

namespace Lastdew
{	
	[GlobalClass]
	public partial class PcData : Resource
	{
		[Export]
		public string Name { get; set; }
		[Export]
		public Texture2D Icon { get; set; }
		[Export]
		public PackedScene PcScene { get; set; }
	}
}
