using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class World : Node3D
	{
		private ClickHandler ClickHandler { get; set; }
		private PcManagerBase PcManager { get; set; }
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
			PcManager = GetNode<PcManagerBase>("%PcManager");
			EnemySpawner = GetNode<EnemySpawner>("%EnemySpawner");
			Hud = GetNode<Hud>("%HUD");
			GameMenu = GetNode<GameMenu>("%GameMenu");
			PauseMenu = GetNode<PauseMenu>("%PauseMenu");
	
			AllPcScenes AllPcs = GD.Load<AllPcScenes>("res://game/world/player-characters/management/all_pc_scenes.tres");
			//MissionTeamData missionTeamData = new MissionTeamData(new int[] { 0, 1, });
			//AllPcsData AllPcs = GD.Load<AllPcsData>("res://game/world/player-characters/management/AllPcsData.tres");
			TeamData teamData = new(AllPcs, new List<int>(){ 0, 1, });
			InventoryManager inventoryManager = new();
			
			PcManager.Initialize(teamData, inventoryManager);
			EnemySpawner.Initialize(teamData);
			Hud.Initialize(teamData);
			GameMenu.Initialize(teamData, inventoryManager);
			
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
