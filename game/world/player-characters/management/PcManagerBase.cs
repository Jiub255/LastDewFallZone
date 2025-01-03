using Godot;

namespace Lastdew
{
	// TODO: Have a different PcManager for scavenging? What about base defense?
	// Or just have different "modes" on this class? 
	public partial class PcManagerBase : Node3D
	{
		private TeamData TeamData { get; set; }
		private int? SelectedIndex { get; set; }
	
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			TeamData = teamData;
			SpawnPcs(inventoryManager);
		}
		
		public override void _Process(double delta)
		{
			base._Process(delta);

			for (int i = 0; i < TeamData.Pcs.Count; i++)
			{
				if (i == SelectedIndex)
				{
					TeamData.Pcs[i].ProcessSelected(delta);
				}
				else
				{
					TeamData.Pcs[i].ProcessUnselected(delta);
				}
			}
		}
	
		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			for (int i = 0; i < TeamData.Pcs.Count; i++)
			{
				if (i == SelectedIndex)
				{
					TeamData.Pcs[i].PhysicsProcessSelected(delta);
				}
				else
				{
					TeamData.Pcs[i].PhysicsProcessUnselected(delta);
				}
			}
		}
	
		public override void _ExitTree()
		{
			base._ExitTree();
			
			foreach (PlayerCharacter pc in TeamData.Pcs)
			{
				pc.ExitTree();
			}
		}
		
		public void SelectPc(PlayerCharacter pc)
		{
			int pcIndex = TeamData.Pcs.IndexOf(pc);
			if (pcIndex == -1)
			{
				SelectedIndex = null;
				GD.PushWarning($"{pc.Name} not found in TeamData.Pcs.");
			}
			else
			{
				SelectedIndex = pcIndex;
			}
		}
		
		public void DeselectPc()
		{
			SelectedIndex = null;
		}
		
		public void MoveTo(MovementTarget movementTarget)
		{
			if (SelectedIndex == null)
			{
				GD.PushWarning($"No selected PC for MoveTo()");
			}
			else
			{
				TeamData.Pcs[(int)SelectedIndex].MoveTo(movementTarget);
			}
		}
		
		private void SpawnPcs(InventoryManager inventoryManager)
		{		
			for (int i = 0; i < TeamData.PcDatas.Count; i++)
			{
				PlayerCharacter pc = (PlayerCharacter)TeamData.PcDatas[i].PcScene.Instantiate();
				CallDeferred(MethodName.AddChild, pc);
				pc.Position += Vector3.Right * i * 3;
				pc.Initialize(inventoryManager);
				TeamData.Pcs.Add(pc);
			}
		}
	}
}
