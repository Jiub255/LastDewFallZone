using Godot;

namespace Lastdew
{
	public partial class Enemy : CharacterBody3D
	{
		// TODO: Start with simple test class. Don't bother with state machine and health component and stuff yet.
		private const float RECALCULATION_DISTANCE_SQUARED = 4.0f;
		
		private enum EnemyState 
		{
			MOVEMENT,
			COMBAT
		}
		private EnemyState State { get; set; } = EnemyState.MOVEMENT;

		private int Health { get; set; } = 3;
		private int Attack { get; } = 1;
		private float MaxSpeed { get; } = 7f;
		private float Acceleration { get; } = 50f;
		private float TurnSpeed { get; } = 360f;
		private float AttackRadius { get; } = 1f;
		private PlayerCharacter Target { get; set; }
		private NavigationAgent3D NavigationAgent { get; set; }
		private EnemyAnimationTree EnemyAnimationTree { get; set; }
		private Vector3 LastTargetPosition { get; set; }
		private StringName BlendAmountPath { get; } = "parameters/movement_blend_tree/idle_move/blend_amount";
		private float AttackTimer { get; set; }
		private float TimeBetweenAttacks { get; } = 1f;

		public override void _Ready()
		{
			base._Ready();
			
			NavigationAgent = GetNode<NavigationAgent3D>("%NavigationAgent3D");
			EnemyAnimationTree = GetNode<EnemyAnimationTree>("%AnimationTree");
			
			NavigationAgent.Connect(
				NavigationAgent3D.SignalName.VelocityComputed,
				Callable.From((Vector3 safeVelocity) => ActuallyMove(safeVelocity)));
		}
		
		public override void _Process(double delta)
		{
			base._Process(delta);
			
			if (State == EnemyState.MOVEMENT)
			{
				Move((float)delta);
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
		
		public void GetHit(int damage)
		{
			Health -= damage;
			if (Health <= 0)
			{
				Health = 0;
				// TODO: Drop loot? Timer to disappear body?
				EnemyAnimationTree.Set("parameters/conditions/Dead", true);
			}
			else
			{
				EnemyAnimationTree.Set("parameters/conditions/GettingHit", true);
			}
		}
		
		public void HitTarget()
		{
			// Hit target (Handled from animation method track)
			Target.GetHit(this, Attack);
		}
		
		public void SetTarget(PlayerCharacter target)
		{
			Target = target;
			Vector3 attackPosition = AttackPosition(Target.GlobalPosition);
			NavigationAgent.TargetPosition = attackPosition;
			LastTargetPosition = attackPosition;
		}

		private void Move(float delta)
		{
			if (WithinRangeOfEnemy())
			{
				State = EnemyState.COMBAT;
				return;
			}
			Animate();
			MoveAndRotate(delta);
		}

		private void MoveAndRotate(float delta)
		{
			Vector3 nextPosition = NavigationAgent.GetNextPathPosition();
			this.RotateToward(nextPosition, TurnSpeed * delta);
			
			Vector3 direction = (nextPosition - GlobalPosition).Normalized();
			Accelerate(direction * MaxSpeed, Acceleration * delta);
		}

		private void Accelerate(Vector3 targetVelocity, float accelerationAmount)
		{
			NavigationAgent.Velocity = Velocity.MoveToward(targetVelocity, accelerationAmount);
		}

		private void Animate()
		{
			float blendAmount = Mathf.Clamp(Velocity.Length() / MaxSpeed, 0, 1);
			EnemyAnimationTree.Set(BlendAmountPath, blendAmount);
		}
		
		private void ActuallyMove(Vector3 safeVelocity)
		{
			Velocity = safeVelocity;
			MoveAndSlide();
		}
		
		private void Fight(float delta)
		{
			// Count down attack timer.
			AttackTimer -= delta;
			if (AttackTimer < 0)
			{
				// TODO: How to handle actual combat? Just have them trade hit/get hit animations?
				// Then just take damage after each hit?
				// Do this for now at least. 
				// Reset timer.
				AttackTimer = TimeBetweenAttacks;
				// If target out of range, move back to movement state.
				if (!WithinRangeOfEnemy())
				{
					State = EnemyState.MOVEMENT;
				}
				
				// Attack animation
				EnemyAnimationTree.Set("parameters/conditions/Attacking", true);
				// If target dies, choose new target (nearest?) and move toward/attack them.
				
			}
		}
		
		private bool WithinRangeOfEnemy()
		{
			return GlobalPosition.DistanceSquaredTo(AttackPosition(Target.GlobalPosition)) < AttackRadius * AttackRadius;
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
	}
}
