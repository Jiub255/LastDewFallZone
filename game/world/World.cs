using Godot;
using System.Collections.Generic;

namespace Lastdew
{	
	public partial class World : Node3D
	{
		private ClickHandler ClickHandler { get; set; }
		private PcManager PcManager { get; set; }
		// TODO: Just for testing for now, probably have a different setup eventually.
		private EnemySpawner EnemySpawner { get; set; }
		private Hud Hud { get; set; }
		private GameMenu GameMenu { get; set; }
		private PauseMenu PauseMenu { get; set; }
		
		public override void _Ready()
		{
			base._Ready();
	
			Camera camera = GetNode<Camera>("%CameraRig");
			ClickHandler = camera.ClickHandler;
			PcManager = GetNode<PcManager>("%PcManager");
			EnemySpawner = GetNode<EnemySpawner>("%EnemySpawner");
			Hud = GetNode<Hud>("%HUD");
			GameMenu = GetNode<GameMenu>("%GameMenu");
			PauseMenu = GetNode<PauseMenu>("%PauseMenu");
	
			AllPcScenes AllPcs = GD.Load<AllPcScenes>("res://game/world/player-characters/management/AllPcScenes.cs");
			MissionTeamData missionTeamData = new MissionTeamData(new int[] { 0, 1, });
			TeamData teamData = new TeamData(AllPcs, new List<int>(){ 0, 1, });
			InventoryManager inventoryManager = new();
			PcManager.Initialize(missionTeamData, inventoryManager);
			EnemySpawner.Initialize(missionTeamData);
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
}
