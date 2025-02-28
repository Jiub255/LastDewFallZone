using Godot;

namespace Lastdew
{
	public partial class Hud : Menu
	{
		private TeamData TeamData { get; set; }
		private PackedScene PcButtonScene { get; set; } = (PackedScene)GD.Load(UIDs.PC_BUTTON);
		private HBoxContainer ButtonParent { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			ButtonParent = GetNode<HBoxContainer>("%HBoxContainer");
		}
		
		public override void Open()
		{
			foreach (Node node in ButtonParent.GetChildren())
			{
				if (node is PcButton button)
				{
					button.SetHealthBars();
				}
			}
			base.Open();
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
			foreach (PlayerCharacter pc in TeamData.Pcs)
			{
				PcButton buttonInstance = PcButtonScene.Instantiate() as PcButton;
				ButtonParent.CallDeferred(HBoxContainer.MethodName.AddChild, buttonInstance);
				// This line is the only reason TeamData has to inherit RefCounted
				buttonInstance.CallDeferred(PcButton.MethodName.Setup, pc, TeamData);
			}
		}
	}
}
