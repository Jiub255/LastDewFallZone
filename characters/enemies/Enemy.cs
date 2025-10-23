using System.Linq;
using Godot;
using Godot.Collections;

namespace Lastdew
{
	public partial class Enemy : CharacterBody3D
	{
        private EnemyTarget _target;
        private Node3D _slot;
        private EnemyState _state = EnemyState.MOVEMENT;

        private const float RECALCULATION_DISTANCE_SQUARED = 0.25f;
		private const float A_LITTLE_BIT = 1f;
        private const string MOVEMENT_BLEND_TREE_NAME = "movement_blend_tree";
        private const string ATTACK_ANIM_NAME = "CharacterArmature|Punch_Right";
		private const string GETTING_HIT_ANIM_NAME = "CharacterArmature|HitRecieve_2";
		private const string DEATH_ANIM_NAME = "CharacterArmature|Death";
		private const float SIGHT_DISTANCE = 20f;

        private enum EnemyState 
		{
			MOVEMENT,
			COMBAT,
			DEAD
		}

		private EnemyState State
		{
			get => _state;
			set
			{
				this.PrintDebug($"{Name}'s state set to {value}");
				_state = value;
			}
		}
		// TODO: Get the following from stats/enemy data resource later.
        private int Health { get; set; } = 5;
		private static int Attack => 10;
		private static float MaxSpeed => 4f;
		private static float Acceleration => 35f;
		private static float TurnSpeed => 360f;
		private static float AttackRadius => 1f;

        private EnemyTarget Target
		{
			get => _target;
            set
            {
                this.PrintDebug($"{Name}'s target set to {(value == null ? "null" : value.PC.Name)}");
				_target = value;
                if (value == null)
				{
					return;
				}
				NavigationAgent.TargetPosition = value.AttackPosition;
				LastTargetPosition = value.AttackPosition;
            }
        }
        private NavigationAgent3D NavigationAgent { get; set; }
		private AnimationTree EnemyAnimationTree { get; set; }
		private AnimationNodeStateMachinePlayback AnimStateMachine { get; set; }
		private Vector3 LastTargetPosition { get; set; }
		private static StringName BlendAmountPath => "parameters/movement_blend_tree/idle_move/blend_amount";
		private float AttackTimer { get; set; }
		private static float TimeBetweenAttacks => 2.5f;
		private float CheckForTargetTimer { get; set; }
		private static float TimeBetweenChecks => 1f;
		
		//private float Delta { get; set; }
		private class EnemyTarget(PlayerCharacter pc, Vector3 combatDirection)
		{
			public PlayerCharacter PC { get; } = pc;
			private Vector3 CombatDirection { get; } = combatDirection;
			public Vector3 AttackPosition => PC.GlobalPosition + CombatDirection * AttackRadius;
		}
		
		public override void _Ready()
		{
			base._Ready();

			NavigationAgent = GetNode<NavigationAgent3D>("%NavigationAgent3D");
			EnemyAnimationTree = GetNode<AnimationTree>("%AnimationTree");

			NavigationAgent.Connect(
				NavigationAgent3D.SignalName.VelocityComputed,
				Callable.From((Vector3 safeVelocity) => Move(safeVelocity)));

			AnimStateMachine = (AnimationNodeStateMachinePlayback)EnemyAnimationTree.Get("parameters/playback");
		}
		
		public override void _Process(double delta)
		{
			base._Process(delta);
            //this.PrintDebug($"{Name}'s current node: {AnimStateMachine.GetCurrentNode()}");

            if (Target == null)
			{
                CheckForTargetTimer -= (float)delta;
                if (CheckForTargetTimer <= 0)
                {
	                CheckForTargetTimer = TimeBetweenChecks;
	                Target = FindNearestPC();
                }
                
                return;
            }

            switch (State)
            {
	            case EnemyState.MOVEMENT:
		            MovementProcess((float)delta);
		            break;
	            case EnemyState.COMBAT:
		            CombatProcess((float)delta);
		            break;
	            case EnemyState.DEAD:
	            default:
		            break;
            }
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			if (Target == null)
			{
                return;
            }

			RecalculateTargetPositionIfTargetMovedEnough();
		}
		
		/// <returns>true if hit killed enemy</returns>
		public bool GetHit(int damage, PlayerCharacter attackingPC)
		{
			Health -= damage;
            this.PrintDebug($"{Name} hit for {damage} damage, health: {Health}");
            if (Health <= 0)
			{
				Health = 0;
				Die();
				return true;
			}
			// TODO: Not sure if this does anything at the moment. When is Target null?
			// Figure it out later.
			//Target /*??*/= attackingPC;
			AnimStateMachine.Travel(GETTING_HIT_ANIM_NAME);
			return false;
		}

		// Called from animation method track
		// Does it need to be public?
		public void HitTarget()
		{
			bool pcIncapacitated = Target.PC.GetHit(this, Attack);
			if (pcIncapacitated)
			{
				Target = FindNearestPC(Target.PC);
            }
		}

		// TODO: Inform all PCs that this enemy is dead, so they look for a new target or set it to null.
		private void Die()
		{
			AnimStateMachine.Travel(DEATH_ANIM_NAME);
			// TODO: Drop loot? Timer to disappear body?
			State = EnemyState.DEAD;
			CollisionLayer = 0;
			NavigationAgent.AvoidanceEnabled = false;
		}

		private void MovementProcess(float delta)
		{
			if (WithinRangeOfEnemy())
			{
				this.PrintDebug($"Within range of enemy");
				State = EnemyState.COMBAT;
				EnemyAnimationTree.Set(BlendAmountPath, 0);
				return;
			}
			Animate();
			Vector3 nextPosition = NavigationAgent.GetNextPathPosition();
			Accelerate(delta, nextPosition);
			Rotate(delta, nextPosition);
		}

		private void Animate()
		{
			float blendAmount = Mathf.Clamp(Velocity.Length() / MaxSpeed, 0, 1);
			EnemyAnimationTree.Set(BlendAmountPath, blendAmount);
		}
		
		private void Rotate(float delta, Vector3 targetToFace)
		{
			this.RotateToward(targetToFace, TurnSpeed * delta);
		}

		private void Accelerate(float delta, Vector3 nextPosition)
		{
			Vector3 direction = (nextPosition - GlobalPosition).Normalized();
            Vector3 targetVelocity = direction * MaxSpeed;
            float accelerationAmount = Acceleration * delta;
            /*NavigationAgent.*/Velocity = Velocity.MoveToward(targetVelocity, accelerationAmount);
			MoveAndSlide();
            //Delta = delta;
		}
		
		private void Move(Vector3 safeVelocity)
		{
			Velocity = safeVelocity;
			MoveAndSlide();
			//MoveAndCollide(safeVelocity * Delta);
		}
		
		private void CombatProcess(float delta)
		{
			if (OutOfRangeOfEnemy())
			{
				State = EnemyState.MOVEMENT;
				AnimStateMachine.Travel(MOVEMENT_BLEND_TREE_NAME);
				return;
			}

			Rotate(delta, Target.PC.GlobalPosition);
			
			AttackTimer -= delta;
			// TODO: Make sure not in "get hit" animation before starting attack. Probably just introduce "GETTING_HIT" state.
            if (AttackTimer < 0 /* && AnimStateMachine.GetCurrentNode() == MOVEMENT_BLEND_TREE_NAME */)
			{
				AttackTimer = TimeBetweenAttacks;
				
				// Attack animation (Calls HitTarget from animation)
				AnimStateMachine.Travel(ATTACK_ANIM_NAME);
                this.PrintDebug($"{Name}'s travel to attack animation just called. Current node: {AnimStateMachine.GetCurrentNode()}");
            }
		}
		
		private bool WithinRangeOfEnemy()
		{
			//return GlobalPosition.DistanceSquaredTo(Target.PC.GlobalPosition) < AttackRadius * AttackRadius + A_LITTLE_BIT / 2;
			return /* Target != null && */ NavigationAgent.IsNavigationFinished();
		}
		
		private bool OutOfRangeOfEnemy()
		{
			float distanceSquared = GlobalPosition.DistanceSquaredTo(Target.AttackPosition);
			return /* Target != null && */ distanceSquared > A_LITTLE_BIT;
		}
		
		private void RecalculateTargetPositionIfTargetMovedEnough()
		{
			Vector3 attackPosition = Target.AttackPosition;
			if (attackPosition.DistanceSquaredTo(LastTargetPosition) > RECALCULATION_DISTANCE_SQUARED)
			{
				NavigationAgent.TargetPosition = attackPosition;
				LastTargetPosition = attackPosition;
			}
		}
		
		private EnemyTarget FindNearestPC(PlayerCharacter currentTarget = null)
        {
            Array<Dictionary> results = SphereCastForNearbyPcs();

            System.Collections.Generic.IEnumerable<PlayerCharacter> pcs = results
	            .Where(d => (CollisionObject3D)d["collider"] is PlayerCharacter)
	            .Select(x => (PlayerCharacter)x["collider"])
	            .OrderBy(y => y.GlobalPosition.DistanceTo(GlobalPosition));

            foreach (PlayerCharacter pc in pcs)
            {
	            Vector3? openCombatDirection = pc.GetOpenCombatDirection();
	            if (openCombatDirection.HasValue)
	            {
		            return new EnemyTarget(pc, openCombatDirection.Value);
	            }
            }

            return null;
        }

        private Array<Dictionary> SphereCastForNearbyPcs()
        {
            PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
            SphereShape3D sphereShape = new() { Radius = SIGHT_DISTANCE };
            PhysicsShapeQueryParameters3D query = new()
            {
                ShapeRid = sphereShape.GetRid(),
                CollideWithBodies = true,
                Transform = new Transform3D(Basis.Identity, GlobalPosition),
                Exclude = new Array<Rid>(new Rid[1] { GetRid() }),
                CollisionMask = 0b10
            };

            Array<Dictionary> result = spaceState.IntersectShape(query);
            return result;
        }
    }
}
