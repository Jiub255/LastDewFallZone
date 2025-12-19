using Godot;
using System;

namespace Lastdew
{
	public partial class ClickHandlerBuild : ClickHandler
	{
		//public event Action<BuildingData> OnPlacedBuilding;
		[Signal]
		public delegate void OnPlacedBuildingEventHandler(BuildingData buildingData);
		
		/// <summary>
		/// Degrees/second
		/// </summary>
		private const float ROTATION_SPEED = 90;
		
		public Building3D Building3D { get; set; }
		public Building Building { get; set; }

		public override void PhysicsProcess(double delta)
		{
			if (Building3D == null || !Building3D.IsInsideTree())
			{
				return;
			}

			RotateBuilding(delta);
			RaycastFromMouse();
			if (!IsColliding())
			{
				return;
			}

			Vector3 position = GetCollisionPoint();
			Building3D.GlobalPosition = position;
		}
		
		public override void UnhandledInput(InputEvent @event)
		{
			if (@event.IsLeftClick())
			{
				HandleClickBuild();
			}
		}

		public void ExitMode()
		{
			Building = null;
			Building3D?.QueueFree();
		}

		private void RotateBuilding(double delta)
		{
			if (Input.IsActionPressed(InputNames.ROTATE_CLOCKWISE))
			{
				Building3D.RotateY(-Mathf.DegToRad(ROTATION_SPEED * (float)delta));
			}
			if (Input.IsActionPressed(InputNames.ROTATE_COUNTER_CLOCKWISE))
			{
				Building3D.RotateY(Mathf.DegToRad(ROTATION_SPEED * (float)delta));
			}
		}

		private void HandleClickBuild()
		{
			if (Building3D == null || Building3D.Overlapping)
			{
				return;
			}
			
			Building3D.SetBuilding();
			BuildingData data = new(Building.GetUid(), Building3D.Transform);
			EmitSignal(SignalName.OnPlacedBuilding, data);
			//OnPlacedBuilding?.Invoke(data);
			Building3D = null;
		}
	}
}
