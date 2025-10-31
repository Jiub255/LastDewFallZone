using Godot;
using System;

namespace Lastdew
{
	public partial class PathNode : Node
	{
		private int _x;
		private int _y;

		public int GCost;
		public int HCost;
		public int FCost;

		public PathNode PrecedingNode;

		public PathNode(int x, int y)
		{
			this._x = x;
			this._y = y;
		}
	}
}
