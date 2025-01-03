using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public class TeamData
	{
		// ------------------------ DATA TO SAVE ------------------------
		
		public List<int> PcIndexes { get; } = new List<int>();

		// --------------------------------------------------------------

		public List<PlayerCharacter> Pcs { get; } = new List<PlayerCharacter>();
		public List<PcData> PcDatas
		{
			get
			{
				List<PcData> pcDatas = new();
				foreach (int index in PcIndexes)
				{
					pcDatas.Add(AllPcs.PcDatas[index]);
				}
				return pcDatas;
			}
		}
		
		private AllPcsData AllPcs { get; }
		
		public TeamData(AllPcsData allPcs, List<int> pcIndexes)
		{
			AllPcs = allPcs;
			PcIndexes = pcIndexes;
		}
	}
}
