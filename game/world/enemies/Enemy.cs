using Godot;
using System;

namespace Lastdew
{
	public partial class Enemy : CharacterBody3D
	{
		// TODO: Start with simple test class. Don't bother with state machine and health component and stuff yet.
		private const float RECALCULATION_DISTANCE_SQUARED = 4.0f;
		private const float ATTACK_RANGE = 10.0f;
		
		private enum EnemyState 
		{
			MOVEMENT,
			COMBAT
		}
		private EnemyState State { get; set; } = EnemyState.MOVEMENT;
		
		private int Health { get; set; }
		private int Attack { get; set;}
		private float MaxSpeed { get; set;}
		private float TurnSpeed { get; set;}
		private float Acceleration { get; set;}
		private PlayerCharacter Target { get; set; }
		private NavigationAgent3D NavigationAgent { get; set; }
		private EnemyAnimationTree EnemyAnimationTree { get; set; }
		private Vector3 LastTargetPosition { get; set; }
		private StringName BlendAmountPath { get; } = "parameters/movement_blend_tree/idle_move/blend_amount";
		private float TestTimer { get; set; } = 1f;

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
			TestTimer -= delta;
			if (TestTimer < 0)
			{
				// TODO: How to handle actual combat? Just have them trade hit/get hit animations?
				// Then just take damage after each hit?
				// Do this for now at least. 
				// Reset timer.
				TestTimer = 1f;
				// If target out of range, move back to movement state.
				if (!WithinRangeOfEnemy())
				{
					State = EnemyState.MOVEMENT;
				}
				// Hit target
				// Attack animation
				// If target dies, choose new target (nearest?) and move toward/attack them.
				
			}
		}
		
		private bool WithinRangeOfEnemy()
		{
			return GlobalPosition.DistanceSquaredTo(Target.GlobalPosition) < ATTACK_RANGE;
		}
		
		private void RecalculateTargetPositionIfTargetMovedEnough()
		{
			if (Target.GlobalPosition.DistanceSquaredTo(LastTargetPosition) > RECALCULATION_DISTANCE_SQUARED)
			{
				NavigationAgent.TargetPosition = Target.GlobalPosition;
				LastTargetPosition = Target.GlobalPosition;
			}
		}
		
		private void SetTarget(PlayerCharacter target)
		{
			Target = target;
			LastTargetPosition = target.GlobalPosition;
		}
	}
}
