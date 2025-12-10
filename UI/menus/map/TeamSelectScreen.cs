using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Lastdew
{
	public partial class TeamSelectScreen : MarginContainer
	{
		public event Action<PackedScene, List<PcSaveData>> OnStartPressed;

		private int _index;
		
		private TeamData TeamData { get; set; }
		private LocationData LocationData { get; set; }
		
		private LocationInfoUi LocationInfoUi { get; set; }
		private SfxButton AddButton { get; set; }
		private SfxButton StartButton { get; set; }
		private VBoxContainer PcDisplayParent { get; set; }
		private CharacterSelector CharacterSelector { get; set; }
		private PackedScene PcDisplayScene { get; set; } = GD.Load<PackedScene>(Uids.PC_DISPLAY);
		private List<PlayerCharacter> UnselectedPcs { get; set; } = [];
		private List<PlayerCharacter> SelectedPcs { get; set; } = [];
		private int Index
		{
		    get => _index;
		    set 
		    {
				_index = value;
				CharacterSelector.SetupDisplay(value);
		    }
		}

		public override void _Ready()
		{
			base._Ready();
			
			LocationInfoUi = GetNode<LocationInfoUi>("%LocationInfo");
			AddButton = GetNode<SfxButton>("%AddButton");
			StartButton = GetNode<SfxButton>("%StartButton");
			PcDisplayParent = GetNode<VBoxContainer>("%PcDisplayParent");
			CharacterSelector = GetNode<CharacterSelector>("%CharacterSelector");

			AddButton.Pressed += AddPcToTeam;
			StartButton.Pressed += StartScavenging;
			CharacterSelector.Previous.Pressed += PreviousPc;
			CharacterSelector.Next.Pressed += NextPc;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			AddButton.Pressed -= AddPcToTeam;
			StartButton.Pressed -= StartScavenging;
			CharacterSelector.Previous.Pressed -= PreviousPc;
			CharacterSelector.Next.Pressed -= NextPc;}

		public void Setup(TeamData teamData, LocationData locationData)
		{
			TeamData = teamData;
			
			foreach (Node node in PcDisplayParent.GetChildren())
			{
				node.QueueFree();
			}
			UnselectedPcs = teamData.Pcs.ToList();
			SelectedPcs.Clear();
			
			LocationData = locationData;
			LocationInfoUi.Setup(
				locationData.Name,
				locationData.Image,
				locationData.Description);
			CharacterSelector.Initialize(UnselectedPcs);
			Show();
		}

		private void AddPcToTeam()
		{
			PlayerCharacter pc = SetupPcDisplay();
			SelectedPcs.Add(pc);
			UnselectedPcs.Remove(pc);
			StartButton.Disabled = false;
			if (UnselectedPcs.Count == 0)
			{
				CharacterSelector.Disable();
				AddButton.Disabled = true;
				AddButton.Visible = false;
			}
			else
			{
				Index = (Index + UnselectedPcs.Count - 1) % UnselectedPcs.Count;
			}
		}

		private void RemoveFromTeam(PcDisplay display)
		{
			SelectedPcs.Remove(display.Pc);
			UnselectedPcs.Add(display.Pc);
			display.OnRemovePc -= RemoveFromTeam;
			display.QueueFree();
			if (UnselectedPcs.Count == 1)
			{
				Index = 0;
				AddButton.Disabled = false;
				AddButton.Visible = true;
				CharacterSelector.Enable();
			}

			if (SelectedPcs.Count == 0)
			{
				StartButton.Disabled = true;
			}
		}

		private PlayerCharacter SetupPcDisplay()
		{
			PcDisplay pcDisplay = (PcDisplay)PcDisplayScene.Instantiate();
			PcDisplayParent.CallDeferred(Node.MethodName.AddChild, pcDisplay);
			PlayerCharacter pc = UnselectedPcs[Index];
			pcDisplay.Initialize(pc);
			pcDisplay.OnRemovePc += RemoveFromTeam;
			return pc;
		}

		private void StartScavenging()
		{
			List<PcSaveData> selectedPcDatas = SelectedPcs.Select(pc => new PcSaveData(pc)).ToList();
			TeamData.UnusedPcDatas.Clear(); 
			foreach (PcSaveData pcData in UnselectedPcs.Select(pc => new PcSaveData(pc)))
			{
				TeamData.UnusedPcDatas.Add(pcData);
			}
			OnStartPressed?.Invoke(LocationData.Scene, selectedPcDatas);
		}
		
		private void PreviousPc()
		{
			Index = (Index - 1 + UnselectedPcs.Count) % UnselectedPcs.Count;
		}
		
		private void NextPc()
		{
			Index = (Index + 1) % UnselectedPcs.Count;
		}
	}
}
