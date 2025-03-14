using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Lastdew
{
	/// <summary>
	/// TODO: Add RemoveFromTeam() method,
	/// deal with UnselectedPcs getting emptied by adding to team,
	/// disable start button until SelectedPcs in non-empty.
	/// </summary>
	public partial class TeamSelectScreen : MarginContainer
	{
		public event Action<PackedScene, List<PcSaveData>> OnStartPressed;

		private int _index;
		
		private TeamData TeamData { get; set; }
		private LocationData LocationData { get; set; }
		private LocationInfoUI LocationInfo { get; set; }
		private Button AddButton { get; set; }
		private Button StartButton { get; set; }
		private VBoxContainer PcDisplayParent { get; set; }
		private CharacterSelector CharacterSelector { get; set; }
		private PackedScene PcDisplayScene { get; set; } = GD.Load<PackedScene>(UIDs.PC_DISPLAY);
		private List<PlayerCharacter> UnselectedPcs { get; set; } = new List<PlayerCharacter>();
		private List<PlayerCharacter> SelectedPcs { get; set; } = new List<PlayerCharacter>();
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
			
			LocationInfo = GetNode<LocationInfoUI>("%LocationInfo");
			AddButton = GetNode<Button>("%AddButton");
			StartButton = GetNode<Button>("%StartButton");
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
			LocationInfo.Setup(
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
		}

		private PlayerCharacter SetupPcDisplay()
		{
			PcDisplay pcDisplay = (PcDisplay)PcDisplayScene.Instantiate();
			PcDisplayParent.CallDeferred(VBoxContainer.MethodName.AddChild, pcDisplay);
			PlayerCharacter pc = UnselectedPcs[Index];
			pcDisplay.Initialize(pc.Icon, pc.Name);
			return pc;
		}

		private void StartScavenging()
		{
			List<PcSaveData> selectedPcDatas = new();
			foreach (PlayerCharacter pc in SelectedPcs)
			{
				PcSaveData pcData = new(pc);
				selectedPcDatas.Add(pcData);
			}
			foreach (PlayerCharacter pc in UnselectedPcs)
			{
				PcSaveData pcData = new(pc);
				TeamData.UnusedPcDatas.Clear();
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
