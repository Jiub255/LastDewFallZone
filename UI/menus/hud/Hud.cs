using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class Hud : Menu
	{
		private TeamData TeamData { get; set; }
		private PackedScene PcButtonScene { get; set; } = (PackedScene)GD.Load(Uids.PC_BUTTON);
		private HBoxContainer ButtonParent { get; set; }
		private List<PcButton> PcButtons { get; set; } = [];
	
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
            ClearPcButtons();
            foreach (PlayerCharacter pc in TeamData.Pcs)
            {
	            if (PcButtonScene.Instantiate() is not PcButton pcButton)
                {
	                GD.PushError("PcButton scene did not instantiate correctly.");
	                continue;
                }
                ButtonParent.CallDeferred(Node.MethodName.AddChild, pcButton);
                pcButton.CallDeferred(PcButton.MethodName.Setup, pc);
                pcButton.OnSelectPc += TeamData.SelectPc;
                PcButtons.Add(pcButton);
            }
		}

        private void ClearPcButtons()
        {
			foreach (PcButton pcButton in PcButtons)
			{
				pcButton.OnSelectPc -= TeamData.SelectPc;
				pcButton.QueueFree();
			}
        }
    }
}
