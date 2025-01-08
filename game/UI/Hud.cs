using Godot;

namespace Lastdew
{	
	public partial class Hud : CanvasLayer
	{
		private TeamData TeamData { get; set; }
		private PackedScene PcButtonScene { get; set; } = (PackedScene)GD.Load("res://game/UI/pc_button.tscn");
		private HBoxContainer Hbox { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			Hbox = GetNode<HBoxContainer>("%HBoxContainer");
		}
		
		public void Initialize(TeamData teamData)
		{
			// TODO: For testing only, gonna pass this data from team selection screen eventually.
			TeamData = teamData;
			TeamData.OnPcsInstantiated += Setup;
			
			/* foreach (int index in TeamData.TeamIndexes)
			{
				PcButton buttonInstance = PcButtonScene.Instantiate() as PcButton;
				Texture2D icon = TeamData.Pcs.PcDatas[index].Icon;
				string name = TeamData.Pcs.PcDatas[index].Name;
				Hbox.CallDeferred(HBoxContainer.MethodName.AddChild, buttonInstance);
				buttonInstance.CallDeferred(PcButton.MethodName.Setup, icon, name);
			} */
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			TeamData.OnPcsInstantiated -= Setup;
		}

		private void Setup()
		{
			foreach (PlayerCharacter pc in TeamData.Pcs)
			{
				PcButton buttonInstance = PcButtonScene.Instantiate() as PcButton;
				Texture2D icon = pc.Icon;
				string name = pc.Name;
				Hbox.CallDeferred(HBoxContainer.MethodName.AddChild, buttonInstance);
				buttonInstance.CallDeferred(PcButton.MethodName.Setup, icon, name);
			}
		}
	}
}
