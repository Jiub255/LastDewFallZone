using Godot;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lastdew
{
	public partial class MissionSummaryMenu : Menu
	{
		[Signal]
		public delegate void DonePressedEventHandler();
		
		private VBoxContainer ItemDisplayParent { get; set; }
		private VBoxContainer PcDisplayParent { get; set; }
		private SfxButton DoneButton { get; set; }
		private PackedScene LootDisplayScene { get; set; } = GD.Load<PackedScene>(Uids.LOOT_DISPLAY);
		private PackedScene PcDisplayScene { get; set; } = GD.Load<PackedScene>(Uids.PC_INFO_DISPLAY);
		private ExperienceFormula Formula { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			ItemDisplayParent = GetNode<VBoxContainer>("%ItemDisplayParent");
			PcDisplayParent = GetNode<VBoxContainer>("%PcDisplayParent");
			DoneButton = GetNode<SfxButton>("%DoneButton");

			DoneButton.Connect(BaseButton.SignalName.Pressed, Callable.From(Close));
		}

		public void Initialize(ExperienceFormula formula)
		{
			Formula = formula;
		}

		public void Setup(ReadOnlyCollection<PlayerCharacter> pcs, MissionData missionData)
		{
			SetupItems(missionData.ItemAmounts);
			SetupPcInfo(pcs, missionData.InitialPcStats);
		}

		private void SetupItems(Dictionary<Texture2D, string> itemAmounts)
		{
			ClearItems();
			
			foreach ((Texture2D icon, string amountAndName) in itemAmounts)
			{
				LootDisplay display = LootDisplayScene.Instantiate() as LootDisplay;
				display?.SetupDisplay(icon, amountAndName);
				ItemDisplayParent.AddChildDeferred(display);
			}
		}

		private void ClearItems()
		{
			foreach (Node child in ItemDisplayParent.GetChildren())
			{
				child.QueueFree();
			}
		}

		private void SetupPcInfo(
			ReadOnlyCollection<PlayerCharacter> pcs,
			Dictionary<string, (int beginningExp, int beginningInjury)> initialPcStats)
		{
			ClearPcs();
			
			foreach (PlayerCharacter pc in pcs)
			{
				int beginningExp = initialPcStats[pc.Data.Name].beginningExp;
				int beginningInjury = initialPcStats[pc.Data.Name].beginningInjury;
				
				PcInfoDisplay display = PcDisplayScene.Instantiate() as PcInfoDisplay;
				PcDisplayParent.AddChildDeferred(display);
				display?.Setup(
					pc,
					Formula,
					beginningExp,
					beginningInjury);
			}
		}

		private void ClearPcs()
		{
			foreach (Node child in PcDisplayParent.GetChildren())
			{
				child.QueueFree();
			}
		}
	}
}
