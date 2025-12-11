using System;
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class Hud : Menu
	{
		public event Action<PlayerCharacter> OnCenterOnPc;
		
		private TeamData TeamData { get; set; }
		private PackedScene PcButtonScene { get; set; } = (PackedScene)GD.Load(Uids.PC_BUTTON);
		private HBoxContainer PcButtonParent { get; set; }
		private PackedScene LootedItemDisplayScene { get; set; } = (PackedScene)GD.Load(Uids.LOOTED_ITEM_DISPLAY);
		private VBoxContainer LootedItemsParent { get; set; }
		private List<PcButton> PcButtons { get; set; } = [];
		public SfxButton Build { get; private set; }
		public SfxButton Craft { get; private set; }
		public SfxButton Character { get; private set; }
		public SfxButton Map { get; private set; }
		public SfxButton Main { get; private set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			PcButtonParent = GetNode<HBoxContainer>("%PcButtonParent");
			LootedItemsParent = GetNode<VBoxContainer>("%LootedItemsParent");
			Build = GetNode<SfxButton>("%Build");
			Craft = GetNode<SfxButton>("%Craft");
			Character = GetNode<SfxButton>("%Character");
			Map = GetNode<SfxButton>("%Map");
			Main = GetNode<SfxButton>("%Main");
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
                pcButton.OnCenterOnPc += CenterOnPc;
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
                pcButton.OnCenterOnPc -= CenterOnPc;
				pcButton.QueueFree();
			}
			PcButtons.Clear();
        }

        private void CenterOnPc(PlayerCharacter pc)
        {
	        OnCenterOnPc?.Invoke(pc);
        }
    }
}
