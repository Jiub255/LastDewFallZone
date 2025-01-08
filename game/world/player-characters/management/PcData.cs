using Godot;

namespace Lastdew
{	
	/// <summary>
	/// TODO: Put this data in PlayerCharacter instead? 
	/// </summary>
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
