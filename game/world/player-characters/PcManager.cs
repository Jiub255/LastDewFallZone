using Godot;

namespace Lastdew
{	
	// TODO: Does this need to be a Node? Can it just be a plain class?
	public partial class PcManager : Node3D
	{
		// TODO: Setup way to load this data. Eventually from save file, but maybe a resource for now?
		private MissionTeamData MissionTeamData { get; set; }
		
		public override void _Process(double delta)
		{
			base._Process(delta);
			
			foreach (PlayerCharacter pc in MissionTeamData.UnselectedPcs)
			{
				pc.ProcessUnselected(delta);
			}
			
			MissionTeamData.SelectedPc?.ProcessSelected(delta);
		}
	
		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
			
			foreach (PlayerCharacter pc in MissionTeamData.UnselectedPcs)
			{
				pc.PhysicsProcessUnselected(delta);
			}
			
			MissionTeamData.SelectedPc?.PhysicsProcessSelected(delta);
		}
	
		public override void _ExitTree()
		{
			base._ExitTree();
			
			foreach (PlayerCharacter pc in MissionTeamData.UnselectedPcs)
			{
				pc.ExitTree();
			}
		}
	
		public void Initialize(MissionTeamData missionTeamData, InventoryManager inventoryManager)
		{
			MissionTeamData = missionTeamData;
			SpawnPcs(inventoryManager);
		}
		
		public void SpawnPcs(InventoryManager inventoryManager)
		{		
			foreach (int index in MissionTeamData.TeamIndexes)
			{
				PlayerCharacter pc = (PlayerCharacter)MissionTeamData.Pcs.PcDatas[index].PcScene.Instantiate();
				CallDeferred(MethodName.AddChild, pc);
				pc.Position += Vector3.Right * index * 3;
				pc.Initialize(inventoryManager);
				MissionTeamData.UnselectedPcs.Add(pc);
			}
		}
		
		public void SelectPc(PlayerCharacter pc)
		{
			if (MissionTeamData.SelectedPc != null)
			{
				if (MissionTeamData.SelectedPc == pc)
				{
					return;
				}
				MissionTeamData.UnselectedPcs.Add(MissionTeamData.SelectedPc);
			}
			MissionTeamData.UnselectedPcs.Remove(pc);
			MissionTeamData.SelectedPc = pc;
			this.PrintDebug($"Selected PC: {MissionTeamData.SelectedPc.Name}");
			foreach (PlayerCharacter unselected in MissionTeamData.UnselectedPcs)
			{
				this.PrintDebug($"Unselected PC: {unselected.Name}");
			}
		}
		
		public void DeselectPc()
		{
			if (MissionTeamData.SelectedPc != null)
			{
				MissionTeamData.UnselectedPcs.Add(MissionTeamData.SelectedPc);
				MissionTeamData.SelectedPc = null;
			}
		}
		
		public void MoveTo(MovementTarget movementTarget)
		{
			MissionTeamData.SelectedPc?.MoveTo(movementTarget);
		}
	}
}
