using System.Linq;
using Godot;
using Godot.Collections;

namespace Lastdew
{
	public class EnemyStateIdle(Enemy enemy) : EnemyState(enemy)
	{
		private const float TIME_BETWEEN_CHECKS = 0.25f;
		private float CheckForTargetTimer { get; set; }
		
		public override void EnterState()
		{
			Enemy.Target = null;
			Enemy.SetBlendAmount(0f);
		}

		public override void ProcessState(float delta)
		{
			CheckForTargetTimer += delta;
			if (CheckForTargetTimer > TIME_BETWEEN_CHECKS)
			{
				CheckForTargetTimer = 0;
				FindNearestPc();
			}
		}
		
		private void FindNearestPc()
		{
			Array<Dictionary> sphereCastResults = SphereCast();

			PlayerCharacter[] pcs = sphereCastResults
				.Where(d => (CollisionObject3D)d["collider"] is PlayerCharacter)
				.Select(x => (PlayerCharacter)x["collider"])
				.OrderBy(y => y.GlobalPosition.DistanceTo(Enemy.GlobalPosition))
				.ToArray();

			if (pcs.Length == 0)
			{
				return;
			}
			
			Enemy.Target = new EnemyTarget(
				pcs[0],
				Vector3.Forward.Rotated(Vector3.Up, GD.Randf() * Mathf.Tau));
			ChangeState(EnemyStateNames.MOVEMENT);
		}

		private Array<Dictionary> SphereCast()
		{
			PhysicsDirectSpaceState3D spaceState = Enemy.GetWorld3D().DirectSpaceState;
			SphereShape3D sphereShape = new() { Radius = Enemy.Data.SightDistance };
			PhysicsShapeQueryParameters3D query = new()
			{
				ShapeRid = sphereShape.GetRid(),
				CollideWithBodies = true,
				Transform = new Transform3D(Basis.Identity, Enemy.GlobalPosition),
				Exclude = new Array<Rid>(new Rid[1] { Enemy.GetRid() }),
				CollisionMask = 0b10
			};

			Array<Dictionary> results = spaceState.IntersectShape(query);
			return results;
		}
	}
}
