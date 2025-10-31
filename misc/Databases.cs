using Godot;

namespace Lastdew
{
	public static class Databases
	{
	    public static readonly Craftables Craftables = GD.Load<Craftables>(UiDs.CRAFTABLES);
        public static readonly AllPcDatas PcDatas = GD.Load<AllPcDatas>(UiDs.ALL_PC_DATAS);
	}
}
