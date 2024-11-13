using Godot;

[GlobalClass]
public partial class PcsData : Resource
{
	[Export]
	public PcData[] PcDatas { get; set; }
}
