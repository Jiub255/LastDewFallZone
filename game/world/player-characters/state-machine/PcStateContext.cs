using Godot;

namespace Lastdew
{
	public class PcStateContext
	{
		private const float SIGHT_DISTANCE = 20f;
		
		public NavigationAgent3D NavigationAgent { get; }
		public PcAnimationTree PcAnimationTree { get; }
		public AnimationNodeStateMachinePlayback AnimStateMachine { get; }
		public Vector3 Position
		{
			get => PC.Position;
		}
		public Vector3 GlobalPosition
		{
			get => PC.GlobalPosition;
		}
		public float Speed
		{
			get => PC.Velocity.Length();
		}
		public InventoryManager InventoryManager { get; }
		
		private PlayerCharacter PC { get; }
		
		public PcStateContext(PlayerCharacter pc, InventoryManager inventoryManager)
		{
			PC = pc;
			InventoryManager = inventoryManager;
			
			NavigationAgent = pc.GetNode<NavigationAgent3D>("%NavigationAgent3D");
			PcAnimationTree = pc.GetNode<PcAnimationTree>("%AnimationTree");
			AnimStateMachine = (AnimationNodeStateMachinePlayback)PcAnimationTree.Get("parameters/playback");
		}
		
		public void Move(Vector3 velocity)
		{
			PC.Velocity = velocity;
			PC.MoveAndSlide();
		}
		
		public void RotateToward(Vector3 nextPosition, float turnAmount)
		{
			PC.RotateToward(nextPosition, turnAmount);
		}
		
		public void Accelerate(Vector3 targetVelocity, float accelerationAmount)
		{
			NavigationAgent.Velocity = PC.Velocity.MoveToward(targetVelocity, accelerationAmount);
		}
		
		public Enemy FindNearestEnemy(Enemy currentTarget)
		{
			PhysicsDirectSpaceState3D spaceState = PC.GetWorld3D().DirectSpaceState;
			SphereShape3D sphereShape = new(){ Radius = SIGHT_DISTANCE };
			PhysicsShapeQueryParameters3D query = new()
			{
				ShapeRid = sphereShape.GetRid(),
				CollideWithBodies = true,
				Transform = new Transform3D(Basis.Identity, PC.GlobalPosition),
				Exclude = new Godot.Collections.Array<Rid>(new Rid[1]{ PC.GetRid() }),
				CollisionMask = 0b100
			};
			
			Godot.Collections.Array<Godot.Collections.Dictionary> result = spaceState.IntersectShape(query);

			Enemy closest = null;
			foreach (Godot.Collections.Dictionary dict in result)
			{
				CollisionObject3D collider = (CollisionObject3D)dict["collider"];
				//this.PrintDebug($"Collider: {collider?.Name}"); 
				if (collider is Enemy enemy)
				{
					if (enemy != currentTarget)
					{
						if (closest == null)
						{
							closest = enemy;
						}
						else if (PC.GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition) < 
							PC.GlobalPosition.DistanceSquaredTo(closest.GlobalPosition))
						{
							closest = enemy;
						}
					}
				}
			}
			return closest;
		}
		
		public void DisablePC()
		{
			PC.CollisionLayer = 0;
			NavigationAgent.AvoidanceEnabled = false;
		}
	}
}
