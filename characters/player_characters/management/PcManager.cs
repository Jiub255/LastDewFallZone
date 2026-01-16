using System;
using System.Collections.Generic;
using System.Linq;
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
		
		public async Task SpawnPcs(
			List<PcSaveData> pcSaveDatas,
			EntranceExit entranceExit,
			ExperienceFormula formula)
		{
			ClearPcs();
			int i = 0;
			foreach (PcSaveData pcSaveData in pcSaveDatas)
			{
				PlayerCharacter pc = (PlayerCharacter)PcScene.Instantiate();
				this.AddChildDeferred(pc);
				pc.Position = entranceExit.SpawnPoints[i];
				pc.Rotation = entranceExit.Rotation + new Vector3(0f, Mathf.Pi, 0f);
				i++;
				// Wait a frame so that pc.Initialize() isn't called before pc.Ready()
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				pc.Initialize(TeamData.Inventory, pcSaveData, formula);
				SubscribePcEvents(pc);
				TeamData.AddPc(pc);
			}

			TeamData.SelectedIndex = null;
			TeamData.MenuSelectedIndex = 0;
		}

		private void ClearPcs()
		{
			foreach (PlayerCharacter pc in GetChildren().OfType<PlayerCharacter>())
			{
				UnsubscribePcEvents(pc);
				pc.QueueFree();
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

		private void SpendAmmo()
		{
			TeamData.Inventory.Ammo--;
		}

		private void SubscribePcEvents(PlayerCharacter pc)
		{
			pc.OnLooted += InvokeOnLooted;
			pc.OnSpendAmmo += SpendAmmo;
			TeamData.Inventory.OnOutOfAmmo += pc.TemporarilyUnequipGun;
			TeamData.Inventory.OnNotOutOfAmmoAnymore += pc.ReequipGun;
		}

		private void UnsubscribePcEvents(PlayerCharacter pc)
		{
			pc.OnLooted -= InvokeOnLooted;
			pc.OnSpendAmmo -= SpendAmmo;
			TeamData.Inventory.OnOutOfAmmo -= pc.TemporarilyUnequipGun;
			TeamData.Inventory.OnNotOutOfAmmoAnymore -= pc.ReequipGun;
		}
	}
}
