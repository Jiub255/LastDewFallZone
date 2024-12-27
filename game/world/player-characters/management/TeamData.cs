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
		public List<PackedScene> PcScenes
		{
			get
			{
				List<PackedScene> pcScenes = new();
				foreach (int index in PcIndexes)
				{
					pcScenes.Add(AllPcs.PcScenes[index]);
				}
				return pcScenes;
			}
		}
		
		private AllPcScenes AllPcs { get; }
		
		public TeamData(AllPcScenes allPcs, List<int> pcIndexes)
		{
			AllPcs = allPcs;
			PcIndexes = pcIndexes;
		}
	}
}
