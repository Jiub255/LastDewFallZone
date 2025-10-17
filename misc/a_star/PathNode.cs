using Godot;
using System;

namespace Lastdew
{
	public partial class PathNode : Node
	{
		private int x;
		private int y;

		public int gCost;
		public int hCost;
		public int fCost;

		public PathNode PrecedingNode;

		public PathNode(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}
}
