using Godot;
using System;

namespace Lastdew
{	
	public partial class ClickHandler : RayCast3D
	{
		//public event Action<BuildingData> OnPlacedBuilding;
		[Signal]
		public delegate void OnPlacedBuildingEventHandler(BuildingData buildingData);
		public event Action<PlayerCharacter> OnClickedPc;
		public event Action<MovementTarget> OnClickedMoveTarget;

		/// <summary>
		/// Degrees/second
		/// </summary>
		private const float ROTATION_SPEED = 90;
		
		// TODO: Base this off of zoom distance?
		private static float RayLength => 2000;
		private Viewport Viewport { get; set; }
		private Camera3D Camera { get; set; }

		private delegate void HandleClickDelegate();
		private HandleClickDelegate _handleClickDelegate;
		private bool _buildMode;
		public Building3D Building3D { get; set; }
		public Building Building { get; set; }

		public bool BuildMode
		{
			get => _buildMode;
			set
			{
				_buildMode = value;
				if (_buildMode)
				{
					_handleClickDelegate = HandleClickBuild;
					CollisionMask = 0b10000; // Ground
				}
				else
				{
					_handleClickDelegate = HandleClick;
					CollisionMask = 0b11110; // Ground, Loot, Enemy, PC
					Building3D?.QueueFree();
				}
			}
		}
	
		public override void _Ready()
		{
			base._Ready();
	
			Viewport = GetViewport();
			Camera = Viewport.GetCamera3D();
			_handleClickDelegate = HandleClick;
		}

		public override void _PhysicsProcess(double delta)
		{
			if (!BuildMode || Building3D == null || !Building3D.IsInsideTree())
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

		public override void _UnhandledInput(InputEvent @event)
		{
			base._UnhandledInput(@event);

			if (@event.IsLeftClick())
			{
				_handleClickDelegate?.Invoke();
			}
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
	
		private void HandleClick()
		{
			GodotObject godotObject = RaycastFromMouse();
			if (godotObject is not CollisionObject3D collider)
			{
				return;
			}
			
			uint layerIndex = collider.CollisionLayer;
			switch (layerIndex)
			{
				// Player Character
				case 0b10:
					OnClickedPc?.Invoke((PlayerCharacter)collider);
					break;
				// Enemy
				case 0b100:
					Enemy enemy = (Enemy)collider;
					MovementTarget movementTargetEnemy = new(enemy.Position, enemy);
					OnClickedMoveTarget?.Invoke(movementTargetEnemy);
					break;
				// Loot
				case 0b1000:
					LootContainer lootContainer = (LootContainer)collider;
					MovementTarget movementTargetLoot = new(lootContainer.LootingPosition, lootContainer);
					OnClickedMoveTarget?.Invoke(movementTargetLoot);
					break;
				// Ground
				case 0b10000:
					Vector3 collisionPoint = IsColliding() ? GetCollisionPoint() : Vector3.Zero;
					MovementTarget movementTargetGround = new(collisionPoint);
					OnClickedMoveTarget?.Invoke(movementTargetGround);
					break;
			}
		}

		private GodotObject RaycastFromMouse()
		{
			Vector3 rayEnd = ToLocal(Camera.ProjectRayNormal(Viewport.GetMousePosition()) * RayLength);
			TargetPosition = rayEnd;
			ForceRaycastUpdate();
			return GetCollider();
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
