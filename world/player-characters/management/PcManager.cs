using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	// TODO: Have a different PcManager for scavenging? What about base defense?
	// Or just have different "modes" on this class? 
	public partial class PcManager : Node3D
	{
		private TeamData TeamData { get; set; }
		private AllPcScenes AllPcs { get; set; }
		
		public void Initialize(TeamData teamData)
		{
			TeamData = teamData;
			AllPcs = GD.Load<AllPcScenes>("res://world/player-characters/management/all_pc_scenes.tres");
		}
		
		public override void _Process(double delta)
		{
			base._Process(delta);

			if (TeamData != null)
			{
				for (int i = 0; i < TeamData.Pcs.Count; i++)
				{
					if (i == TeamData.SelectedIndex)
					{
						TeamData.Pcs[i].ProcessSelected(delta);
					}
					else
					{
						TeamData.Pcs[i].ProcessUnselected(delta);
					}
				}

				if (Input.IsActionJustPressed(InputNames.DESELECT))
				{
					DeselectPc();
				}
			}
		}
	
		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			if (TeamData != null)
			{
				for (int i = 0; i < TeamData.Pcs.Count; i++)
				{
					if (i == TeamData.SelectedIndex)
					{
						TeamData.Pcs[i].PhysicsProcessSelected(delta);
					}
					else
					{
						TeamData.Pcs[i].PhysicsProcessUnselected(delta);
					}
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
		
		public void MoveTo(MovementTarget movementTarget)
		{
			if (TeamData.SelectedIndex != null)
			{
				TeamData.Pcs[(int)TeamData.SelectedIndex].MoveTo(movementTarget);
			}
		}
		
		public void SelectPc(PlayerCharacter pc)
		{
			TeamData.SelectPc(pc);
		}
		
		public void SpawnPcs(InventoryManager inventoryManager, List<PcSaveData> pcSaveDatas)
		{
			ClearPcs();
			int i = 0;
			foreach (PcSaveData pcSaveData in pcSaveDatas)
			{
				PlayerCharacter pc = (PlayerCharacter)AllPcs[pcSaveData.Name].Instantiate();
				CallDeferred(PcManager.MethodName.AddChild, pc);
				// TODO: Add a spawn location for pcs.
				pc.Position += Vector3.Right * i * 3;
				i++;
				pc.Initialize(inventoryManager, pcSaveData);
				TeamData.AddPc(pc);
			}
			
			TeamData.SendPcsInstantiatedSignal();
		}
		
		private void DeselectPc()
		{
			TeamData.SelectedIndex = null;
		}

		private void ClearPcs()
		{
			foreach (Node node in GetChildren())
			{
				if (node is PlayerCharacter pc)
				{
					pc.QueueFree();
				}
			}
			TeamData.ClearPcs();
		}
	}
}
