using Godot;
using System;

namespace Lastdew
{	
	public partial class ClickHandler : RayCast3D
	{
		public event Action<PlayerCharacter> OnClickedPc;
		public event Action<MovementTarget> OnClickedMoveTarget;
		
		// TODO: Base this off of zoom distance?
		private float RayLength { get; } = 2000;
		private Viewport Viewport { get; set; }
		private Camera3D Camera { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
	
			Viewport = GetViewport();
			Camera = Viewport.GetCamera3D();
		}
	
		public override void _Process(double delta)
		{
			base._Process(delta);
			
			if (Input.IsActionJustPressed(InputNames.SELECT))
			{
				GodotObject collisionObject = RaycastFromMouse(Viewport.GetMousePosition());
				if (collisionObject != null)
				{
					HandleClick((CollisionObject3D)collisionObject);
				}
			}
		}
	
		private GodotObject RaycastFromMouse(Vector2 mousePosition)
		{
			Vector3 rayEnd = ToLocal(Camera.ProjectRayNormal(mousePosition) * RayLength);
			TargetPosition = rayEnd;
			ForceRaycastUpdate();
			return GetCollider();
		}
	
		private void HandleClick(CollisionObject3D collider)
		{
			uint layerIndex = collider.CollisionLayer;
			
			switch (layerIndex)
			{
				// Player Character
				case 0b10:
					OnClickedPc?.Invoke((PlayerCharacter)collider);
					//this.PrintDebug($"Clicked pc");
					break;
				// Enemy
				case 0b100:
					Enemy enemy = (Enemy)collider;
					MovementTarget movementTargetEnemy = new(enemy.Position, enemy);
					OnClickedMoveTarget?.Invoke(movementTargetEnemy);
					//this.PrintDebug($"Clicked enemy");
					break;
				// Loot
				case 0b1000:
					LootContainer lootContainer = (LootContainer)collider;
					MovementTarget movementTargetLoot = new(lootContainer.LootingPosition, lootContainer);
					OnClickedMoveTarget?.Invoke(movementTargetLoot);
					//this.PrintDebug($"Clicked loot");
					break;
				// Ground
				case 0b10000:
					Vector3 collisionPoint = IsColliding() ? GetCollisionPoint() : Vector3.Zero;
					MovementTarget movementTargetGround = new(collisionPoint);
					OnClickedMoveTarget?.Invoke(movementTargetGround);
					//this.PrintDebug($"Clicked ground at {collisionPoint}");
					break;
				default:
                    break;
            }
		}
	}
}
