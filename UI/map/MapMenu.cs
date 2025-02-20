using System;
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class MapMenu : Menu
	{
		public event Action<PackedScene, List<PcSaveData>> OnStartScavenging;
		
		private TeamData TeamData { get; set; }
		private MapScreen MapScreen { get; set; }
		private TeamSelectScreen TeamSelectScreen { get; set; }
		private Button SelectTeamButton { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			MapScreen = GetNode<MapScreen>("%MapScreen");
			TeamSelectScreen = GetNode<TeamSelectScreen>("%TeamSelectScreen");
			SelectTeamButton = GetNode<Button>("%SelectTeamButton");

			SelectTeamButton.Pressed += OpenTeamSelectMenu;
			TeamSelectScreen.OnStartPressed += StartScavenging;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			SelectTeamButton.Pressed -= OpenTeamSelectMenu;
			TeamSelectScreen.OnStartPressed -= StartScavenging;
		}

		public override void Close()
		{
			base.Close();
			
			if (TeamData.SelectedIndex != null)
			{
				TeamData.MenuSelectedIndex = (int)TeamData.SelectedIndex;
			}
			MapScreen.Show();
			TeamSelectScreen.Hide();
		}
		
		public void Initialize(TeamData teamData)
		{
			TeamData = teamData;
		}

		private void OpenTeamSelectMenu()
		{
			MapScreen.Hide();
			TeamSelectScreen.Setup(TeamData, MapScreen.LocationData);
		}
		
		private void StartScavenging(PackedScene scene, List<PcSaveData> pcs)
		{
			OnStartScavenging?.Invoke(scene, pcs);
		}
	}
}
