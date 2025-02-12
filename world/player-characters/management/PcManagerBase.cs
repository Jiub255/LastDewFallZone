using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	// TODO: Have a different PcManager for scavenging? What about base defense?
	// Or just have different "modes" on this class? 
	public partial class PcManagerBase : Node3D
	{
		private TeamData TeamData { get; set; }
		private bool Started { get; set; }
		
		public void Initialize(TeamData teamData, InventoryManager inventoryManager, List<PcSaveData> pcSaveDatas)
		{
			TeamData = teamData;
			TeamData.SpawnPcs(this, inventoryManager, pcSaveDatas);
			Started = true;
		}
		
		public override void _Process(double delta)
		{
			base._Process(delta);

			if (Started)
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

			if (Started)
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
		
		private void DeselectPc()
		{
			TeamData.SelectedIndex = null;
		}
	}
}
