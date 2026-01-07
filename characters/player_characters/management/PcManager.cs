using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

namespace Lastdew
{
	public partial class PcManager : Node3D
	{
		public event Action<Texture2D, string> OnLooted;
		
		private const float SPACE_BETWEEN_PCS = 1.5f;
		private const float MOUSE_MOVEMENT_THRESHOLD = 10f;

		private TeamData TeamData { get; set; }
		private PackedScene PcScene { get; } = GD.Load<PackedScene>(Uids.PC_BASE);
		private Viewport Viewport { get; set; }
		private bool DeselectHeld { get; set; }
		private Vector2 StartingMousePosition { get; set; }

		public void Initialize(TeamData teamData)
		{
			Viewport = GetViewport();
			TeamData = teamData;
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
				DeselectHeld = true;
				StartingMousePosition = Viewport.GetMousePosition();
			}

			if (Input.IsActionJustReleased(InputNames.DESELECT))
			{
				DeselectHeld = false;
				if (Viewport.GetMousePosition().DistanceTo(StartingMousePosition) < MOUSE_MOVEMENT_THRESHOLD)
				{
					DeselectPc();
				}
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
		
		public void SpawnPcs(InventoryManager inventoryManager, List<PcSaveData> pcSaveDatas, Vector3 spawnPosition)
		{
			ClearPcs();
			int i = 0;
			foreach (PcSaveData pcSaveData in pcSaveDatas)
			{
				PlayerCharacter pc = (PlayerCharacter)PcScene.Instantiate();
				this.AddChildDeferred(pc);
				// TODO: Add a spawn location for pcs. Probably do a whole different system for spawning pcs eventually.
				pc.Position = spawnPosition + Vector3.Right * SPACE_BETWEEN_PCS * i;
				i++;
				pc.Initialize(inventoryManager, pcSaveData);
				pc.OnLooted += InvokeOnLooted;
				TeamData.AddPc(pc);
			}

			TeamData.SelectedIndex = null;
			TeamData.MenuSelectedIndex = 0;
		}

		public void ClearPcs()
		{
			foreach (Node node in GetChildren())
			{
				if (node is PlayerCharacter pc)
				{
					pc.OnLooted -= InvokeOnLooted;
					pc.QueueFree();
				}
			}
			TeamData.ClearPcs();
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

		private void InvokeOnLooted(Texture2D icon, string name)
		{
			OnLooted?.Invoke(icon, name);
		}
	}
}
