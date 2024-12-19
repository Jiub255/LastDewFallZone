using Godot;

namespace Lastdew
{
	public partial class Enemy : CharacterBody3D
	{
		// TODO: Start with simple test class. Don't bother with state machine and health component and stuff yet.
		private const float RECALCULATION_DISTANCE_SQUARED = 0.25f;
		private const float A_LITTLE_BIT = 1.25f;
		
		private enum EnemyState 
		{
			MOVEMENT,
			COMBAT
		}
		private EnemyState State { get; set; } = EnemyState.MOVEMENT;

		private int Health { get; set; } = 3;
		private int Attack { get; } = 1;
		private float MaxSpeed { get; } = 4f;
		private float Acceleration { get; } = 35f;
		private float TurnSpeed { get; } = 360f;
		private float AttackRadius { get; } = 1f;
		private PlayerCharacter Target { get; set; }
		private NavigationAgent3D NavigationAgent { get; set; }
		private EnemyAnimationTree EnemyAnimationTree { get; set; }
		private Vector3 LastTargetPosition { get; set; }
		private StringName BlendAmountPath { get; } = "parameters/movement_blend_tree/idle_move/blend_amount";
		private float AttackTimer { get; set; }
		private float TimeBetweenAttacks { get; } = 3.5f;

		public override void _Ready()
		{
			base._Ready();
			
			NavigationAgent = GetNode<NavigationAgent3D>("%NavigationAgent3D");
			EnemyAnimationTree = GetNode<EnemyAnimationTree>("%AnimationTree");
			
			NavigationAgent.Connect(
				NavigationAgent3D.SignalName.VelocityComputed,
				Callable.From((Vector3 safeVelocity) => ActuallyMove(safeVelocity)));
				
			EnemyAnimationTree.Connect(
				AnimationTree.SignalName.AnimationFinished,
				Callable.From((string animationName) => ResetAnimationParameter(animationName)));
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
				/* EnemyAnimationTree.CallDeferred(
					AnimationTree.MethodName.Set,
					"parameters/conditions/Dead",
					false); */
			}
			else
			{
				EnemyAnimationTree.Set("parameters/conditions/GettingHit", true);
				/* EnemyAnimationTree.CallDeferred(
					AnimationTree.MethodName.Set,
					"parameters/conditions/GettingHit",
					false); */
			}
			this.PrintDebug($"Enemy GetHit called");
		}
		
		// Called from animation method track
		public void HitTarget()
		{
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
				this.PrintDebug($"Enemy moving to combat state.");
				State = EnemyState.COMBAT;
				EnemyAnimationTree.Set(BlendAmountPath, 0);
				return;
			}
			Animate();
			MoveAndRotate(delta);
		}

		private void Animate()
		{
			float blendAmount = Mathf.Clamp(Velocity.Length() / MaxSpeed, 0, 1);
			EnemyAnimationTree.Set(BlendAmountPath, blendAmount);
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
		
		private void ActuallyMove(Vector3 safeVelocity)
		{
			Velocity = safeVelocity;
			MoveAndSlide();
		}
		
		private void Fight(float delta)
		{
			if (OutOfRangeOfEnemy())
			{
				this.PrintDebug($"Enemy moving to movement state.");
				State = EnemyState.MOVEMENT;
				return;
			}
			
			AttackTimer -= delta;
			if (AttackTimer < 0)
			{
				AttackTimer = TimeBetweenAttacks;
				
				// Attack animation (Calls HitTarget from animation)
				EnemyAnimationTree.Set("parameters/conditions/Attacking", true);
				/* EnemyAnimationTree.CallDeferred(
					AnimationTree.MethodName.Set,
					"parameters/conditions/Attacking",
					false); */
				
				// TODO: If target dies, choose new target (nearest?) and move toward/attack them.
			}
		}
		
		private bool WithinRangeOfEnemy()
		{
			return NavigationAgent.IsNavigationFinished();
			/*float distanceSquared = GlobalPosition.DistanceSquaredTo(AttackPosition(Target.GlobalPosition));
			return distanceSquared <= AttackRadius * AttackRadius  - A_LITTLE_BIT */;
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
		
		private void ResetAnimationParameter(string animationName)
		{
			if (animationName == "HitRecieve") // Match the "GetHit" animation's name.
			{
				EnemyAnimationTree.Set("parameters/conditions/GettingHit", false);
			}
			else if (animationName == "Punch_Right")
			{
				EnemyAnimationTree.Set("parameters/conditions/Attacking", false);
			}
		}
	}
}
