using Godot;

namespace Lastdew
{	
	[GlobalClass]
	public partial class AllPcsData : Resource
	{
		[Export]
		public PcData[] PcDatas { get; set; }
	}
}
