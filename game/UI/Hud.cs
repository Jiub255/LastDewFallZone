using Godot;

namespace Lastdew
{	
	public partial class Hud : Menu
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
			TeamData = teamData;
			TeamData.OnPcsInstantiated += Setup;
			Setup();
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			TeamData.OnPcsInstantiated -= Setup;
		}

		private void Setup()
		{
			this.PrintDebug($"Setup called");
			foreach (PlayerCharacter pc in TeamData.Pcs)
			{
				PcButton buttonInstance = PcButtonScene.Instantiate() as PcButton;
				Hbox.CallDeferred(HBoxContainer.MethodName.AddChild, buttonInstance);
				buttonInstance.CallDeferred(PcButton.MethodName.Setup, pc, TeamData);
			}
		}
	}
}
