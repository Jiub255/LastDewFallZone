using Godot;
using System;

public partial class World : Node3D
{
	private ClickHandler ClickHandler { get; set; }
	private PcManager PcManager { get; set; }
	
	public override void _Ready()
	{
		base._Ready();
		
		Camera camera = GetNode<Camera>("%CameraRig");
		ClickHandler = camera.ClickHandler;
		PcManager = GetNode<PcManager>("%PcManager");

		ClickHandler.OnClickedPc += PcManager.SelectPc;
		ClickHandler.OnClickedLoot += PcManager.MoveTo;
		ClickHandler.OnClickedGround += PcManager.MoveTo;
	}
}
