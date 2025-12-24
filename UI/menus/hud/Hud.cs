using System;
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class Hud : Menu
	{
		public event Action<PlayerCharacter> OnCenterOnPc;
		
		public SfxButton Build { get; private set; }
		public SfxButton Craft { get; private set; }
		public SfxButton Character { get; private set; }
		public SfxButton Map { get; private set; }
		public SfxButton Main { get; private set; }
		private TeamData TeamData { get; set; }
		private PackedScene PcButtonScene { get; set; } = (PackedScene)GD.Load(Uids.PC_BUTTON);
		private HBoxContainer PcButtonParent { get; set; }
		private PackedScene LootedItemDisplayScene { get; set; } = (PackedScene)GD.Load(Uids.LOOTED_ITEM_DISPLAY);
		private VBoxContainer LootedItemsParent { get; set; }
		private Label Food { get; set; }
		private Label Water { get; set; }
		private List<PcButton> PcButtons { get; set; } = [];
		private Queue<LootedItem> LootedItems { get; } = new();
		private static float TimeBetweenItemShowings => 0.2f;
		private double Timer { get; set; }

		private struct LootedItem(Texture2D icon, string name)
		{
			public Texture2D Icon = icon;
			public string Name = name;
		}

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

		public override void _Process(double delta)
		{
			base._Process(delta);

			if (LootedItems.Count <= 0)
			{
				return;
			}
			
			Timer += delta;
			if (Timer > TimeBetweenItemShowings)
			{
				Timer = 0;
				ShowNextLootedItem();
			}
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			TeamData.Inventory.OnFoodChanged -= SetFood;
			TeamData.Inventory.OnWaterChanged -= SetWater;
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
		
		public void Initialize(TeamData teamData, TimeManager timeManager)
		{
			TeamData = teamData;
			Clock clock = GetNode<Clock>("%Clock");
			Food = GetNode<Label>("%FoodAmount");
			Water = GetNode<Label>("%WaterAmount");
			
			clock.Initialize(timeManager);
			SetFood();
			SetWater();
			TeamData.Inventory.OnFoodChanged += SetFood;
			TeamData.Inventory.OnWaterChanged += SetWater;
		}

		public void Setup()
		{
            ClearPcButtons();
            foreach (PlayerCharacter pc in TeamData.Pcs)
            {
	            if (PcButtonScene.Instantiate() is not PcButton pcButton)
                {
	                GD.PushError("PcButton scene did not instantiate correctly.");
	                continue;
                }
                PcButtonParent.AddChildDeferred(pcButton);
                pcButton.CallDeferred(PcButton.MethodName.Setup, pc);
                pcButton.OnSelectPc += TeamData.SelectPc;
                pcButton.OnCenterOnPc += CenterOnPc;
                PcButtons.Add(pcButton);
            }
		}

		public void AddToQueue(Texture2D icon, string name)
		{
			LootedItems.Enqueue(new LootedItem(icon, name));
		}

		private void ShowNextLootedItem()
		{
			LootedItem item = LootedItems.Dequeue();
			LootedItemDisplay itemDisplay = (LootedItemDisplay)LootedItemDisplayScene.Instantiate();
			LootedItemsParent.AddChildDeferred(itemDisplay);
			itemDisplay.CallDeferred(LootedItemDisplay.MethodName.Setup, item.Icon, item.Name);
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

        private void SetFood()
        {
	      //  Food ??= GetNode<Label>("%FoodAmount");
	        Food.Text = TeamData?.Inventory?.Food.ToString() ?? "0";
        }

        private void SetWater()
        {
	       // Water ??= GetNode<Label>("%WaterAmount");
	        Water.Text = TeamData?.Inventory?.Water.ToString() ?? "0";
        }
    }
}
