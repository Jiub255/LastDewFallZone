using Godot;
using System;

public partial class World : Node3D
{
	private ClickHandler ClickHandler { get; set; }
	private PcManager PcManager { get; set; }
	private Hud Hud { get; set; }
	
	public override void _Ready()
	{
		base._Ready();
		
		Camera camera = GetNode<Camera>("%CameraRig");
		ClickHandler = camera.ClickHandler;
		PcManager = GetNode<PcManager>("%PcManager");
		Hud = GetNode<Hud>("%HUD");

		MissionTeamData missionTeamData = new MissionTeamData(new int[] { 0, 1, });
		PcManager.Setup(missionTeamData);
		Hud.Setup(missionTeamData);
		
		ClickHandler.OnClickedPc += PcManager.SelectPc;
		ClickHandler.OnClickedLoot += PcManager.MoveTo;
		ClickHandler.OnClickedGround += PcManager.MoveTo;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		
		ClickHandler.OnClickedPc -= PcManager.SelectPc;
		ClickHandler.OnClickedLoot -= PcManager.MoveTo;
		ClickHandler.OnClickedGround -= PcManager.MoveTo;
	}
}
