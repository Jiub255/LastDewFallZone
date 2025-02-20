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
		private PackedScene PcDisplayScene { get; set; }
		private List<PcSaveData> Pcs { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			LocationInfo = GetNode<LocationInfoUI>("%LocationInfo");
			AddButton = GetNode<Button>("%AddButton");
			StartButton = GetNode<Button>("%StartButton");
			PcDisplayParent = GetNode<VBoxContainer>("%PcDisplayParent");
			PcDisplayScene = GD.Load<PackedScene>("uid://8b0531d6h835");
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
			foreach (Node node in PcDisplayParent.GetChildren())
			{
				node.QueueFree();
			}
			Pcs.Clear();
			Show();
		}

		private void AddPcToTeam()
		{
			PcDisplay pcDisplay = (PcDisplay)PcDisplayScene.Instantiate();
			PcDisplayParent.CallDeferred(VBoxContainer.MethodName.AddChild, pcDisplay);
			PlayerCharacter pc = TeamData.Pcs[TeamData.MenuSelectedIndex];
			pcDisplay.Initialize(pc.Icon, pc.Name);
			PcSaveData data = new(
				pc.Name,
				pc.Equipment.Head.Name,
				pc.Equipment.Weapon.Name,
				pc.Equipment.Body.Name,
				pc.Equipment.Feet.Name,
				pc.Health.Injury);
			Pcs.Add(data);
		}

		private void StartScavenging()
		{
			OnStartPressed?.Invoke(LocationData.Scene, Pcs);
		}
	}
}
