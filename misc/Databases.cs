using Godot;

namespace Lastdew
{
	public static class Databases
	{
	    public static readonly Craftables CRAFTABLES = GD.Load<Craftables>(UIDs.CRAFTABLES);
        public static readonly AllPcDatas PC_DATAS = GD.Load<AllPcDatas>(UIDs.ALL_PC_DATAS);
	}
}
