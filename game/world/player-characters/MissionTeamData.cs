using Godot;
using System.Collections.Generic;

public class MissionTeamData
{
	// TODO: Keep this array (AllPcs) separate? Maybe as a resource?
	public PackedScene[] AllPcs { get; } = new PackedScene[]
	{
		(PackedScene)GD.Load("res://game/world/player-characters/humans_master.tscn"),
		(PackedScene)GD.Load("res://game/world/player-characters/humans_master2.tscn"),
	};
	
	// TeamIndexes is only used to instantiate from the AllPcs array. 
	// TODO: Eventually populate TeamIndexes from a team selection UI. 
	// Might not need this here in that case? Figure it out when it happens.
	public int[] TeamIndexes { get; init; }
	public List<PlayerCharacter> UnselectedPcs { get; } = new();
	public PlayerCharacter SelectedPc { get; set; }
	
	public MissionTeamData(int[] teamIndexes)
	{
		TeamIndexes = teamIndexes;
	}
}
