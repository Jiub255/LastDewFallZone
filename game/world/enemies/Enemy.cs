using Godot;

namespace Lastdew
{
	public partial class Enemy : CharacterBody3D
	{
		// TODO: Start with simple test class. Don't bother with state machine and health component and stuff yet.
		private const float RECALCULATION_DISTANCE_SQUARED = 0.25f;
		private const float A_LITTLE_BIT = 1.25f;
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
		private EnemyState State { get; set; } = EnemyState.MOVEMENT;

		private int Health { get; set; } = 3;
		private int Attack { get; } = 10;
		private float MaxSpeed { get; } = 4f;
		private float Acceleration { get; } = 35f;
		private float TurnSpeed { get; } = 360f;
		private float AttackRadius { get; } = 1f;
		private PlayerCharacter Target { get; set; }
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
				Callable.From((Vector3 safeVelocity) => ActuallyMove(safeVelocity)));

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
				Fight((float)delta);
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
		
		public void SetTarget(PlayerCharacter target)
		{
			Target = target;
			Vector3 attackPosition = AttackPosition(Target.GlobalPosition);
			NavigationAgent.TargetPosition = attackPosition;
			LastTargetPosition = attackPosition;
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
			Move(delta, nextPosition);
			Rotate(delta, nextPosition);
		}

		private void Animate()
		{
			float blendAmount = Mathf.Clamp(Velocity.Length() / MaxSpeed, 0, 1);
			EnemyAnimationTree.Set(BlendAmountPath, blendAmount);
		}

		private void Move(float delta, Vector3 nextPosition)
		{
			Vector3 direction = (nextPosition - GlobalPosition).Normalized();
			Accelerate(direction * MaxSpeed, Acceleration * delta);
		}
		
		private void Rotate(float delta, Vector3 targetToFace)
		{
			this.RotateToward(targetToFace, TurnSpeed * delta);
		}

		private void Accelerate(Vector3 targetVelocity, float accelerationAmount)
		{
			NavigationAgent.Velocity = Velocity.MoveToward(targetVelocity, accelerationAmount);
		}
		
		private void ActuallyMove(Vector3 safeVelocity)
		{
			Velocity = safeVelocity;
			MoveAndSlide();
		}
		
		private void Fight(float delta)
		{
			if (OutOfRangeOfEnemy())
			{
				State = EnemyState.MOVEMENT;
				return;
			}

			Rotate(delta, Target.GlobalPosition);
			
			AttackTimer -= delta;
			if (AttackTimer < 0)
			{
				AttackTimer = TimeBetweenAttacks;
				
				// Attack animation (Calls HitTarget from animation)
				AnimStateMachine.Travel(ATTACK_ANIM_NAME);
			}
		}
		
		private bool WithinRangeOfEnemy()
		{
			return NavigationAgent.IsNavigationFinished();
		}
		
		private bool OutOfRangeOfEnemy()
		{
			float distanceSquared = GlobalPosition.DistanceSquaredTo(AttackPosition(Target.GlobalPosition));
			return distanceSquared > AttackRadius * AttackRadius  + A_LITTLE_BIT ;
		}
		
		private void RecalculateTargetPositionIfTargetMovedEnough()
		{
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
			PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
			SphereShape3D sphereShape = new(){ Radius = SIGHT_DISTANCE };
			PhysicsShapeQueryParameters3D query = new()
			{
				ShapeRid = sphereShape.GetRid(),
				CollideWithBodies = true,
				Transform = new Transform3D(Basis.Identity, GlobalPosition),
				Exclude = new Godot.Collections.Array<Rid>(new Rid[1]{ GetRid() }),
				CollisionMask = 0b10
			};
			
			Godot.Collections.Array<Godot.Collections.Dictionary> result = spaceState.IntersectShape(query);

			PlayerCharacter closest = null;
			foreach (Godot.Collections.Dictionary dict in result)
			{
				CollisionObject3D collider = (CollisionObject3D)dict["collider"];
				//this.PrintDebug($"Collider: {collider?.Name}"); 
				if (collider is PlayerCharacter pc)
				{
					if (pc != currentTarget)
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
			}
			return closest;
		}
	}
}
