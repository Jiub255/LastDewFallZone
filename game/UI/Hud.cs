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
		
		public void Setup(TeamData teamData)
		{
			// TODO: For testing only, gonna pass this data from team selection screen eventually.
			TeamData = teamData;
			
			foreach (PcData pcData in TeamData.PcDatas)
			{
				PcButton buttonInstance = PcButtonScene.Instantiate() as PcButton;
				Texture2D icon = pcData.Icon;
				string name = pcData.Name;
				Hbox.CallDeferred(HBoxContainer.MethodName.AddChild, buttonInstance);
				buttonInstance.CallDeferred(PcButton.MethodName.Setup, icon, name);
			}
			
			/* foreach (int index in TeamData.TeamIndexes)
			{
				PcButton buttonInstance = PcButtonScene.Instantiate() as PcButton;
				Texture2D icon = TeamData.Pcs.PcDatas[index].Icon;
				string name = TeamData.Pcs.PcDatas[index].Name;
				Hbox.CallDeferred(HBoxContainer.MethodName.AddChild, buttonInstance);
				buttonInstance.CallDeferred(PcButton.MethodName.Setup, icon, name);
			} */
		}
	}
}
