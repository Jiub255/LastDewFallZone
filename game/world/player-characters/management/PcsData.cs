using Godot;

namespace Lastdew
{	
	[GlobalClass]
	public partial class PcsData : Resource
	{
		[Export]
		public PcData[] PcDatas { get; set; }
	}
}
