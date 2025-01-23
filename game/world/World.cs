using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class World : Node3D
	{
		private ClickHandler ClickHandler { get; set; }
		private PcManagerBase PcManager { get; set; }
		private UiManager UI { get; set; }
		
		public override void _Ready()
		{
			base._Ready();
	
			Camera camera = GetNode<Camera>("%CameraRig");
			ClickHandler = camera.ClickHandler;
			PcManager = GetNode<PcManagerBase>("%PcManager");
			UI = GetNode<UiManager>("%UiManager");
			
			ClickHandler.OnClickedPc += PcManager.SelectPc;
			ClickHandler.OnClickedMoveTarget += PcManager.MoveTo;
			UI.MainMenu.OnNewGame += StartNewGame;
			GetTree().Paused = true;
		}
	
		public override void _ExitTree()
		{
			base._ExitTree();
			
			ClickHandler.OnClickedPc -= PcManager.SelectPc;
			ClickHandler.OnClickedMoveTarget -= PcManager.MoveTo;
			UI.MainMenu.OnNewGame -= StartNewGame;
		}
		
		private void StartNewGame(PackedScene packedScene)
		{
			AllPcScenes AllPcs = GD.Load<AllPcScenes>("res://game/world/player-characters/management/all_pc_scenes.tres");
			TeamData teamData = new(AllPcs, new List<int>(){ 0, 1, });
			InventoryManager inventoryManager = new();
			
			Level level = (Level)packedScene.Instantiate();
			CallDeferred(World.MethodName.AddChild, level);
	
			level.Initialize(teamData);
			PcManager.Initialize(teamData, inventoryManager);
			UI.Initialize(teamData, inventoryManager);
		}
	}
}
