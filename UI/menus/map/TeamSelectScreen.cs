using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Lastdew
{
	/// <summary>
	/// TODO: Add RemoveFromTeam() method,
	/// deal with UnselectedPcs getting emptied by adding to team,
	/// disable start button until SelectedPcs is non-empty.
	/// </summary>
	public partial class TeamSelectScreen : MarginContainer
	{
		public event Action<PackedScene, List<PcSaveData>> OnStartPressed;

		private int _index;
		
		private TeamData TeamData { get; set; }
		private LocationData LocationData { get; set; }
		
		private LocationInfoUi LocationInfo { get; set; }
		private Button AddButton { get; set; }
		private Button StartButton { get; set; }
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
			
			LocationInfo = GetNode<LocationInfoUi>("%LocationInfo");
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
			StartButton.Disabled = false;
			if (UnselectedPcs.Count == 0)
			{
				CharacterSelector.Disable();
				AddButton.Disabled = true;
				AddButton.Visible = false;
			}
			else
			{
				// Doing this to avoid setting index below zero because the setter has a method that
				// needs a valid index.
				int index = Index - 1;
				if (index < 0)
				{
					index = UnselectedPcs.Count - 1;
				}
				Index = index;
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
			this.PrintDebug($"Index: {Index}, Pc: {pc.Data.Name}");
			pcDisplay.Initialize(pc);
			pcDisplay.OnRemovePc += RemoveFromTeam;
			return pc;
		}

		private void StartScavenging()
		{
			List<PcSaveData> selectedPcDatas = SelectedPcs.Select(pc => new PcSaveData(pc)).ToList();
			foreach (PcSaveData pcData in UnselectedPcs.Select(pc => new PcSaveData(pc)))
			{
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
