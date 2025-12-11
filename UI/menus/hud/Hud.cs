using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class Hud : Menu
	{
		private TeamData TeamData { get; set; }
		private PackedScene PcButtonScene { get; set; } = (PackedScene)GD.Load(Uids.PC_BUTTON);
		private HBoxContainer PcButtonParent { get; set; }
		private PackedScene LootedItemDisplayScene { get; set; } = (PackedScene)GD.Load(Uids.LOOTED_ITEM_DISPLAY);
		private VBoxContainer LootedItemsParent { get; set; }
		private List<PcButton> PcButtons { get; set; } = [];
	
		public override void _Ready()
		{
			base._Ready();
			
			PcButtonParent = GetNode<HBoxContainer>("%PcButtonParent");
			LootedItemsParent = GetNode<VBoxContainer>("%LootedItemsParent");
		}
		
		public override void Open()
		{
			foreach (Node node in PcButtonParent.GetChildren())
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
                PcButtonParent.CallDeferred(Node.MethodName.AddChild, pcButton);
                pcButton.CallDeferred(PcButton.MethodName.Setup, pc);
                pcButton.OnSelectPc += TeamData.SelectPc;
                PcButtons.Add(pcButton);
            }
		}

		public void ShowLootedItems(Item item, int amount)
		{
			LootedItemDisplay itemDisplay = (LootedItemDisplay)LootedItemDisplayScene.Instantiate();
			LootedItemsParent.CallDeferred(Node.MethodName.AddChild, itemDisplay);
			itemDisplay.CallDeferred(LootedItemDisplay.MethodName.Setup, item, amount);
		}

        private void ClearPcButtons()
        {
			foreach (PcButton pcButton in PcButtons)
			{
				pcButton.OnSelectPc -= TeamData.SelectPc;
				pcButton.QueueFree();
			}
			PcButtons.Clear();
        }
    }
}
