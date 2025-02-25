using Godot;
using System.Collections.Generic;

namespace Lastdew
{	
	public class MissionTeamData
	{
		// TODO: Keep this array (AllPcs) separate? Maybe as a resource?
		/* public PackedScene[] AllPcs { get; } = new PackedScene[]
		{
			(PackedScene)GD.Load("res://game/world/player-characters/humans_master.tscn"),
			(PackedScene)GD.Load("res://game/world/player-characters/humans_master2.tscn"),
		}; */
		// TODO: Keep this array in a separate resource, then keep a reference to that resource here?
		public AllPcsData Pcs { get; private set; } = GD.Load<AllPcsData>("res://game/world/player-characters/PcsData.tres");
		
		// TeamIndexes is only used to instantiate from the AllPcs array. 
		// TODO: Eventually populate TeamIndexes from a team selection UI. 
		// Might not need this here in that case? Figure it out when it happens.
		public int[] TeamIndexes { get; init; }
		// TODO: There has to be a better way to store this data. Maybe just keep a list of all PCs,
		// then just store a nullable int index for which is selected.
		public List<PlayerCharacter> UnselectedPcs { get; } = new();
		public PlayerCharacter SelectedPc { get; set; }
		
		public MissionTeamData(int[] teamIndexes)
		{
			TeamIndexes = teamIndexes;
		}
	}
}
