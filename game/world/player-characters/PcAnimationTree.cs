using Godot;

namespace Lastdew
{	
	public partial class PcAnimationTree : AnimationTree
	{
		public bool Looting { get; set; }
		public bool Attacking { get; set; }
		public bool GettingHit { get; set; }
	}
}
