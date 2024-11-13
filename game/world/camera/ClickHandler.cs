using Godot;
using System;

public partial class ClickHandler : RayCast3D
{
	public event Action<PlayerCharacter> OnClickedPc;
	public event Action<LootContainer> OnClickedLoot;
	public event Action<Vector3> OnClickedGround;

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
			(GodotObject, Vector3) collisionData = RaycastFromMouse(Viewport.GetMousePosition());
			if (collisionData.Item1 != null)
			{
				HandleClick((CollisionObject3D)collisionData.Item1, collisionData.Item2);
			}
		}
	}

	private (GodotObject, Vector3) RaycastFromMouse(Vector2 mousePosition)
	{
		Vector3 rayEnd = ToLocal(Camera.ProjectRayNormal(mousePosition) * RayLength);
		TargetPosition = rayEnd;
		ForceRaycastUpdate();
		Vector3 collisionPoint = IsColliding() ? GetCollisionPoint() : Vector3.Zero;
		return (GetCollider(), collisionPoint);
	}

	private void HandleClick(CollisionObject3D collider, Vector3 collisionPoint)
	{
		uint layerIndex = collider.CollisionLayer;
		
		switch (layerIndex)
		{
			// Player Character
			case 2:
				OnClickedPc?.Invoke((PlayerCharacter)collider);
				this.PrintDebug($"Clicked pc");
				break;
			// Enemy
			case 4:
				// TODO: How to handle enemy click?
				break;
			// Loot
			case 8:
				OnClickedLoot?.Invoke((LootContainer)collider);
				this.PrintDebug($"Clicked loot");
				break;
			// Ground
			case 16:
				OnClickedGround?.Invoke(collisionPoint);
				this.PrintDebug($"Clicked ground");
				break;
		}
	}
}
