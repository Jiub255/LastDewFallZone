using System.Linq;
using Godot;
using Godot.Collections;

namespace Lastdew
{
	public class EnemyStateIdle(Enemy enemy) : EnemyState(enemy)
	{
		private const float SIGHT_DISTANCE = 20f;
		private const float TIME_BETWEEN_CHECKS = 1f;
		private float CheckForTargetTimer { get; set; }
		
		public override void EnterState()
		{
			Enemy.Target = null;
		}

		public override void ProcessState(float delta)
		{
			CheckForTargetTimer += (float)delta;
			if (CheckForTargetTimer > TIME_BETWEEN_CHECKS)
			{
				CheckForTargetTimer = 0;
				FindNearestPC();
			}
		}
		
		private void FindNearestPC()
		{
			Array<Dictionary> results = SphereCastForNearbyPcs();

			System.Collections.Generic.IEnumerable<PlayerCharacter> pcs = results
				.Where(d => (CollisionObject3D)d["collider"] is PlayerCharacter)
				.Select(x => (PlayerCharacter)x["collider"])
				.OrderBy(y => y.GlobalPosition.DistanceTo(Enemy.GlobalPosition));

			foreach (PlayerCharacter pc in pcs)
			{
				Vector3? openCombatDirection = pc.GetOpenCombatDirection();
				if (openCombatDirection.HasValue)
				{
					Enemy.Target = new EnemyTarget(pc, openCombatDirection.Value);
					ChangeState(EnemyStateNames.MOVEMENT);
					return;
				}
			}
		}

		private Array<Dictionary> SphereCastForNearbyPcs()
		{
			PhysicsDirectSpaceState3D spaceState = Enemy.GetWorld3D().DirectSpaceState;
			SphereShape3D sphereShape = new() { Radius = SIGHT_DISTANCE };
			PhysicsShapeQueryParameters3D query = new()
			{
				ShapeRid = sphereShape.GetRid(),
				CollideWithBodies = true,
				Transform = new Transform3D(Basis.Identity, Enemy.GlobalPosition),
				Exclude = new Array<Rid>(new Rid[1] { Enemy.GetRid() }),
				CollisionMask = 0b10
			};

			Array<Dictionary> result = spaceState.IntersectShape(query);
			return result;
		}
	}
}
