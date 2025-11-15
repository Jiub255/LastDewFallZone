using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public partial class PcManager : Node3D
	{
		private const float SPACE_BETWEEN_PCS = 1.5f;
		
		private TeamData TeamData { get; set; }
		private PackedScene PcScene { get; set; }
		
		public void Initialize(TeamData teamData)
		{
			TeamData = teamData;
			PcScene = GD.Load<PackedScene>(UiDs.PC_BASE);
		}
		
		public override void _Process(double delta)
		{
			base._Process(delta);

			if (TeamData == null)
			{
				return;
			}
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
	
		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			if (TeamData == null)
			{
				return;
			}
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
			if (TeamData.SelectedIndex == null)
			{
				return;
			}
			PlayerCharacter selectedPc = TeamData.Pcs[(int)TeamData.SelectedIndex];
			if (selectedPc.Incapacitated)
			{
				TeamData.SelectedIndex = null;
				return;
			}
			selectedPc.MoveTo(movementTarget);
		}
		
		public void SelectPc(PlayerCharacter pc)
		{
			if (pc.Incapacitated)
			{
				return;
			}
			TeamData.SelectPc(pc);
		}
		
		public void SpawnPcs(InventoryManager inventoryManager, List<PcSaveData> pcSaveDatas)
		{
			//this.PrintDebug($"PcManager.SpawnPcs() called");
			ClearPcs();
			int i = 0;
			foreach (PcSaveData pcSaveData in pcSaveDatas)
			{
				// TODO: How to work in the PcData with this part and saving/loading?
				// Just keep their mesh enums as "stats"? Or have each PcData as an unchanging resource? Probably that.
				PlayerCharacter pc = (PlayerCharacter)PcScene.Instantiate();
				CallDeferred(Node.MethodName.AddChild, pc);
				// TODO: Add a spawn location for pcs. Probably do a whole different system for spawning pcs eventually.
				pc.Position += Vector3.Right * SPACE_BETWEEN_PCS * i;
				i++;
				pc.Initialize(inventoryManager, pcSaveData);
				TeamData.AddPc(pc);
			}
			
			TeamData.SendPcsInstantiatedSignal();
		}
		
		private void DeselectPc()
		{
			if (TeamData.SelectedIndex != null)
			{
				int index = (int)TeamData.SelectedIndex;
				TeamData.Pcs[index].SetSelectedIndicator(false);
			}
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
