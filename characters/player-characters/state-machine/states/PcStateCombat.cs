using Godot;
using Godot.Collections;

namespace Lastdew
{
	public class PcStateCombat : PcState
	{
		private const float SIGHT_DISTANCE = 20f;
		private PcCombatSubstate CurrentSubstate { get; set; }
		private System.Collections.Generic.Dictionary<PcCombatSubstateNames, PcCombatSubstate> StatesByEnum { get; } = new();
		
		public PcStateCombat(PlayerCharacter pc) : base(pc)
		{
			SetupSubstates(pc);
		}
	
		public override void EnterState()
		{
			if (Pc.MovementTarget.Target is Enemy enemy)
			{
				ChangeSubstate(PcCombatSubstateNames.WAITING);
			}
			else
			{
				ChangeState(PcStateNames.IDLE);
			}
		}

		public override void ProcessSelected(float delta)
		{
			if (TargetDead())
			{
				// TODO: Look for new target instead. Or just do that in idle (unselected) state?
				//ChangeState(PcStateNames.IDLE);
				TryFindNearestEnemy();
			}
			CurrentSubstate.ProcessSelected(delta);
		}
	
		public override void ProcessUnselected(float delta)
		{
			if (TargetDead())
			{
				// TODO: Look for new target instead. Or just do that in idle (unselected) state?
				//ChangeState(PcStateNames.IDLE);
				TryFindNearestEnemy();
			}
			CurrentSubstate.ProcessUnselected(delta);
		}

		public override void PhysicsProcessSelected(float delta) {}
		
		public override void PhysicsProcessUnselected(float delta) {}
		
		public override void ExitState() {}
		
		public void ExitTree()
		{			
			foreach (PcCombatSubstate state in StatesByEnum.Values)
			{
				state.OnChangeSubstate -= ChangeSubstate;
			}
		}
		
		public void HitEnemy()
		{
			if (CurrentSubstate is not PcStateAttacking attackState)
			{
				GD.PushWarning($"Not in attacking substate. Current substate: {CurrentSubstate.GetType()}");
				return;
			}
			bool targetKilled = attackState.HitEnemy();
			if (targetKilled)
			{
				TryFindNearestEnemy();
			}
		}

        public void GetHit(bool incapacitated)
		{
			if (incapacitated)
			{
				Pc.Incapacitated = true;
				ChangeSubstate(PcCombatSubstateNames.INCAPACITATED);
				return;
			}
			CurrentSubstate.GetHit();
		}
	
		private void SetupSubstates(PlayerCharacter pc)
		{
			PcStateWaiting waiting = new(pc);
			PcStateAttacking attacking = new(pc);
			PcStateGettingHit gettingHit = new(pc);
			PcStateIncapacitated incapacitated = new(pc);
			
			// Populate states dictionary
			StatesByEnum.Add(PcCombatSubstateNames.WAITING, waiting);
			StatesByEnum.Add(PcCombatSubstateNames.ATTACKING, attacking);
			StatesByEnum.Add(PcCombatSubstateNames.GETTING_HIT, gettingHit);
			StatesByEnum.Add(PcCombatSubstateNames.INCAPACITATED, incapacitated);
			
			foreach (PcCombatSubstate state in StatesByEnum.Values)
			{
				state.OnChangeSubstate += ChangeSubstate;
			}
		}

        private void TryFindNearestEnemy()
        {
	        Enemy nearest = null;
	        if (Pc.MovementTarget.Target is Enemy enemy)
	        {
	            nearest = FindNearestEnemy(enemy);
	        }
	        else
	        {
		        nearest = FindNearestEnemy(null);
	        }
	        
            if (nearest != null)
            {
	            Pc.MovementTarget = new MovementTarget(nearest.GlobalPosition, nearest);
                ChangeState(PcStateNames.MOVEMENT);
            }
            else
            {
                ChangeState(PcStateNames.IDLE);
            }
        }
		
		private void ChangeSubstate(PcCombatSubstateNames substateName)
		{
			CurrentSubstate = StatesByEnum[substateName];
			CurrentSubstate?.EnterState();
		}
		
		private Enemy FindNearestEnemy(Enemy currentTarget)
		{
			Array<Dictionary> results = SphereCastForNearbyEnemies();

			Enemy closest = null;
			foreach (Dictionary dict in results)
			{
				CollisionObject3D collider = (CollisionObject3D)dict["collider"];
				//this.PrintDebug($"Collider: {collider?.Name}");
				// TODO: Why not allow currentTarget in finding closest target?
				if (collider is not Enemy enemy || enemy == currentTarget)
				{
					continue;
				}
				if (closest == null || Pc.GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition) <
				    Pc.GlobalPosition.DistanceSquaredTo(closest.GlobalPosition))
				{
					closest = enemy;
				}
			}
			return closest;
		}

		private Array<Dictionary> SphereCastForNearbyEnemies()
		{
			PhysicsDirectSpaceState3D spaceState = Pc.GetWorld3D().DirectSpaceState;
			SphereShape3D sphereShape = new() { Radius = SIGHT_DISTANCE };
			PhysicsShapeQueryParameters3D query = new()
			{
				ShapeRid = sphereShape.GetRid(),
				CollideWithBodies = true,
				Transform = new Transform3D(Basis.Identity, Pc.GlobalPosition),
				Exclude = new Array<Rid>(new Rid[1] { Pc.GetRid() }),
				CollisionMask = 0b100
			};

			Array<Dictionary> result = spaceState.IntersectShape(query);
			return result;
		}

		private bool TargetDead()
		{
			if (Pc.MovementTarget.Target is Enemy enemy)
			{
				return enemy.Health <= 0;
			}
			return false;
		}
	}
}
