using System;
using System.Collections;
using Godot;

namespace Lastdew
{	
	[GlobalClass, Tool]
	public partial class AllPcDatas : Resource, IEnumerable
	{
		[Export]
		public Godot.Collections.Dictionary<string, PcData> PcDatas { get; set; } = new();
		
        public PcData this[string name] => PcDatas[name];
        
		public IEnumerator GetEnumerator()
		{
			return PcDatas.Values.GetEnumerator();
		}
	}
}
