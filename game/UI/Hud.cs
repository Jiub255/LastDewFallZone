using Godot;

public partial class Hud : CanvasLayer
{
	private MissionTeamData MissionTeamData { get; set; }
	private PackedScene PcButtonScene { get; set; } = (PackedScene)GD.Load("res://game/UI/pc_button.tscn");
	private HBoxContainer Hbox { get; set; }

	public override void _Ready()
	{
		base._Ready();
		
		Hbox = GetNode<HBoxContainer>("%HBoxContainer");
	}
	
	public void Setup(MissionTeamData missionTeamData)
	{
		// TODO: For testing only, gonna pass this data from team selection screen eventually.
		MissionTeamData = missionTeamData;
		
		foreach (int index in MissionTeamData.TeamIndexes)
		{
			PcButton buttonInstance = PcButtonScene.Instantiate() as PcButton;
			Texture2D icon = MissionTeamData.Pcs.PcDatas[index].Icon;
			string name = MissionTeamData.Pcs.PcDatas[index].Name;
			Hbox.CallDeferred(HBoxContainer.MethodName.AddChild, buttonInstance);
			buttonInstance.CallDeferred(PcButton.MethodName.Setup, icon, name);
		}
	}
}
