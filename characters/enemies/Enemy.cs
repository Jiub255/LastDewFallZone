using Godot;
using Godot.Collections;

namespace Lastdew
{
	/// <summary>
    /// TODO: When fighting multiple enemies, only the one targeted by the PC does the attack animation,
    /// even though it seems to run the code fine on each enemy. Maybe look into how PC changing targets affects enemies?
    /// </summary>
	public partial class Enemy : CharacterBody3D
	{
        private PlayerCharacter _target;
        private EnemyState _state = EnemyState.MOVEMENT;

        // TODO: Start with simple test class. Don't bother with state machine and health component and stuff yet.
        private const float RECALCULATION_DISTANCE_SQUARED = 0.25f;
		private const float A_LITTLE_BIT = /* 1.25f */2f;
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
        private int Health { get; set; } = 5;
		private int Attack { get; } = 10;
		private float MaxSpeed { get; } = 4f;
		private float Acceleration { get; } = 35f;
		private float TurnSpeed { get; } = 360f;
		private float AttackRadius { get; } = 0.5f;
        private PlayerCharacter Target
		{
			get => _target;
            set
            {
                this.PrintDebug($"{Name}'s target set to {(value == null ? "null" : value.Name)}");
				_target = value;
				Vector3 attackPosition = AttackPosition(value.GlobalPosition);
				NavigationAgent.TargetPosition = attackPosition;
				LastTargetPosition = attackPosition;
            }
        }
        private NavigationAgent3D NavigationAgent { get; set; }
		private AnimationTree EnemyAnimationTree { get; set; }
		private AnimationNodeStateMachinePlayback AnimStateMachine { get; set; }
		private Vector3 LastTargetPosition { get; set; }
		private StringName BlendAmountPath { get; } = "parameters/movement_blend_tree/idle_move/blend_amount";
		private float AttackTimer { get; set; }
		private float TimeBetweenAttacks { get; } = 3.5f;

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
			
			if (State == EnemyState.MOVEMENT)
			{
				MovementProcess((float)delta);
			}
			else if (State == EnemyState.COMBAT)
			{
				CombatProcess((float)delta);
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

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
			Target ??= attackingPC;
			AnimStateMachine.Travel(GETTING_HIT_ANIM_NAME);
			return false;
		}

		// Called from animation method track
		public void HitTarget()
		{
			bool pcIncapacitated = Target.GetHit(this, Attack);
			if (pcIncapacitated)
			{
				Target = FindNearestPC(Target);
            }
		}

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
            NavigationAgent.Velocity = Velocity.MoveToward(targetVelocity, accelerationAmount);
		}
		
		private void Move(Vector3 safeVelocity)
		{
			Velocity = safeVelocity;
			MoveAndSlide();
		}
		
		private void CombatProcess(float delta)
		{
			if (OutOfRangeOfEnemy())
			{
				State = EnemyState.MOVEMENT;
				AnimStateMachine.Travel(MOVEMENT_BLEND_TREE_NAME);
				return;
			}

			Rotate(delta, Target.GlobalPosition);
			
			AttackTimer -= delta;
            //this.PrintDebug($"{Name}'s attack timer: {AttackTimer}");
            if (AttackTimer < 0)
			{
				AttackTimer = TimeBetweenAttacks;
				
				// Attack animation (Calls HitTarget from animation)
				AnimStateMachine.Travel(ATTACK_ANIM_NAME);
                this.PrintDebug($"{Name}'s travel to attack animation just called. Current node: {AnimStateMachine.GetCurrentNode()}");
            }
		}
		
		private bool WithinRangeOfEnemy()
		{
			return /* Target != null || */ NavigationAgent.IsNavigationFinished();
		}
		
		private bool OutOfRangeOfEnemy()
		{
			/* if (Target == null)
			{
                return true;
            } */
			float distanceSquared = GlobalPosition.DistanceSquaredTo(AttackPosition(Target.GlobalPosition));
			return distanceSquared > AttackRadius * AttackRadius + A_LITTLE_BIT;
		}
		
		private void RecalculateTargetPositionIfTargetMovedEnough()
		{
			/* if (Target == null)
			{
                return;
            } */
			Vector3 attackPosition = AttackPosition(Target.GlobalPosition);
			if (attackPosition.DistanceSquaredTo(LastTargetPosition) > RECALCULATION_DISTANCE_SQUARED)
			{
				NavigationAgent.TargetPosition = attackPosition;
				LastTargetPosition = attackPosition;
			}
		}
		
		private Vector3 AttackPosition(Vector3 targetPosition)
		{
			Vector3 direction = (GlobalPosition - targetPosition).Normalized();
			return targetPosition + direction * AttackRadius;
		}
		
		private PlayerCharacter FindNearestPC(PlayerCharacter currentTarget)
        {
            Array<Dictionary> results = SphereCastForNearbyPcs();

            PlayerCharacter closest = null;
            foreach (Dictionary dict in results)
            {
                CollisionObject3D collider = (CollisionObject3D)dict["collider"];
                //this.PrintDebug($"Collider: {collider?.Name}"); 
                if (collider is PlayerCharacter pc && pc != currentTarget)
                {
                    if (closest == null)
                    {
                        closest = pc;
                    }
                    else if (GlobalPosition.DistanceSquaredTo(pc.GlobalPosition) <
                        GlobalPosition.DistanceSquaredTo(closest.GlobalPosition))
                    {
                        closest = pc;
                    }
                }
            }
            return closest;
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
