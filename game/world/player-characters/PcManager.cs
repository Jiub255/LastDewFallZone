using Godot;

public partial class PcManager : Node3D
{
	// TODO: Setup way to load this data. Eventually from save file, but maybe a resource for now?
	private MissionTeamData MissionTeamData { get; set; }
	
	public override void _Ready()
	{
		base._Ready();

		SpawnPcs();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		foreach (PlayerCharacter pc in MissionTeamData.UnselectedPcs)
		{
			pc.ProcessUnselected(delta);
		}
		
		MissionTeamData.SelectedPc?.ProcessSelected(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		
		foreach (PlayerCharacter pc in MissionTeamData.UnselectedPcs)
		{
			pc.PhysicsProcessUnselected(delta);
		}
		
		MissionTeamData.SelectedPc?.PhysicsProcessSelected(delta);
	}
	
	public void SpawnPcs()
	{
		// JUST FOR TESTING, pass this list/array from team selection screen eventually.
		MissionTeamData = new MissionTeamData(new int[] { 0, 1, });
		
		foreach (int index in MissionTeamData.TeamIndexes)
		{
			PlayerCharacter pc = (PlayerCharacter)MissionTeamData.AllPcs[index].Instantiate();
			CallDeferred(MethodName.AddChild, pc);
			pc.Position += Vector3.Right * index * 3;
			GD.Print($"PC Position: {pc.Position}");
			MissionTeamData.UnselectedPcs.Add(pc);
		}
	}
	
	public void SelectPc(PlayerCharacter pc)
	{
		if (MissionTeamData.SelectedPc != null)
		{
			if (MissionTeamData.SelectedPc == pc)
			{
				return;
			}
			MissionTeamData.UnselectedPcs.Add(MissionTeamData.SelectedPc);
		}
		MissionTeamData.UnselectedPcs.Remove(pc);
		MissionTeamData.SelectedPc = pc;
		GD.Print($"Selected PC: {MissionTeamData.SelectedPc.Name}");
	}
	
	public void DeselectPc()
	{
		if (MissionTeamData.SelectedPc != null)
		{
			MissionTeamData.UnselectedPcs.Add(MissionTeamData.SelectedPc);
			MissionTeamData.SelectedPc = null;
		}
	}
	
	public void MoveTo(Vector3 location)
	{
		GD.Print($"Move to location: {location}");
		MissionTeamData.SelectedPc?.MoveTo(location);
	}
	
	public void MoveTo(Node3D target)
	{
		GD.Print($"Move to target: {target}");
		MissionTeamData.SelectedPc?.MoveTo(target);
	}
}
