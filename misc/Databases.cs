using Godot;

namespace Lastdew
{
	public static class Databases
	{
	    public static readonly Craftables Craftables = GD.Load<Craftables>(Uids.CRAFTABLES);
        public static readonly AllPcDatas PcDatas = GD.Load<AllPcDatas>(Uids.ALL_PC_DATAS);
	}
}
