using Godot;
using System;

namespace Lastdew
{
	public partial class ClickHandlerGame : ClickHandler
	{
		public event Action<PlayerCharacter> OnClickedPc;
		public event Action<MovementTarget> OnClickedMoveTarget;

		public override void PhysicsProcess(double delta) {}
		
		public override void UnhandledInput(InputEvent @event)
		{
			if (@event.IsLeftClick())
			{
				HandleClick();
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
	}
}
