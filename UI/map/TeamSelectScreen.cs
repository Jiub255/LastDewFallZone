using System;
using System.Collections.Generic;
using Godot;
	
namespace Lastdew
{
	public partial class TeamSelectScreen : MarginContainer
	{
		public event Action<PackedScene, List<PcSaveData>> OnStartPressed;
				
		private TeamData TeamData { get; set; }
		private LocationData LocationData { get; set; }
		private LocationInfoUI LocationInfo { get; set; }
		private Button AddButton { get; set; }
		private Button StartButton { get; set; }
		private VBoxContainer PcDisplayParent { get; set; }
		private CharacterDisplay CharacterDisplay { get; set; }
		private PackedScene PcDisplayScene { get; set; } = GD.Load<PackedScene>(UIDs.PC_DISPLAY);
		private List<PcSaveData> Pcs { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			LocationInfo = GetNode<LocationInfoUI>("%LocationInfo");
			AddButton = GetNode<Button>("%AddButton");
			StartButton = GetNode<Button>("%StartButton");
			PcDisplayParent = GetNode<VBoxContainer>("%PcDisplayParent");
			CharacterDisplay = GetNode<CharacterDisplay>("%CharacterDisplay");
			Pcs = new List<PcSaveData>();

			AddButton.Pressed += AddPcToTeam;
			StartButton.Pressed += StartScavenging;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			AddButton.Pressed -= AddPcToTeam;
			StartButton.Pressed -= StartScavenging;
		}

		public void Setup(TeamData teamData, LocationData locationData)
		{
			TeamData = teamData;
			LocationData = locationData;
			LocationInfo.Setup(
				locationData.Name,
				locationData.Image,
				locationData.Description);
			CharacterDisplay.Initialize(teamData);
			foreach (Node node in PcDisplayParent.GetChildren())
			{
				node.QueueFree();
			}
			Pcs.Clear();
			Show();
		}

		private void AddPcToTeam()
		{
			PlayerCharacter pc = SetupDisplay();
			PcSaveData data = new(
				pc.Name,
				pc.Equipment.Head.Name,
				pc.Equipment.Weapon.Name,
				pc.Equipment.Body.Name,
				pc.Equipment.Feet.Name,
				pc.Health.Injury);
			Pcs.Add(data);
		}

		private PlayerCharacter SetupDisplay()
		{
			PcDisplay pcDisplay = (PcDisplay)PcDisplayScene.Instantiate();
			PcDisplayParent.CallDeferred(VBoxContainer.MethodName.AddChild, pcDisplay);
			PlayerCharacter pc = TeamData.Pcs[TeamData.MenuSelectedIndex];
			pcDisplay.Initialize(pc.Icon, pc.Name);
			return pc;
		}

		private void StartScavenging()
		{
			OnStartPressed?.Invoke(LocationData.Scene, Pcs);
		}
	}
}
