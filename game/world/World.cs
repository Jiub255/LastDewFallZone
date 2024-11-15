using Godot;

public partial class World : Node3D
{
	private ClickHandler ClickHandler { get; set; }
	private PcManager PcManager { get; set; }
	private Hud Hud { get; set; }
	private GameMenu GameMenu { get; set; }
	private PauseMenu PauseMenu { get; set; }
	
	public override void _Ready()
	{
		base._Ready();

		Camera camera = GetNode<Camera>("%CameraRig");
		ClickHandler = camera.ClickHandler;
		PcManager = GetNode<PcManager>("%PcManager");
		Hud = GetNode<Hud>("%HUD");
		GameMenu = GetNode<GameMenu>("%GameMenu");
		PauseMenu = GetNode<PauseMenu>("%PauseMenu");

		InventoryManager inventoryManager = new();
		MissionTeamData missionTeamData = new MissionTeamData(new int[] { 0, 1, });
		PcManager.Initialize(missionTeamData, inventoryManager);
		Hud.Setup(missionTeamData);
		GameMenu.Initialize(inventoryManager);
		
		ClickHandler.OnClickedPc += PcManager.SelectPc;
		ClickHandler.OnClickedMoveTarget += PcManager.MoveTo;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		
		ClickHandler.OnClickedPc -= PcManager.SelectPc;
		ClickHandler.OnClickedMoveTarget -= PcManager.MoveTo;
	}
}
