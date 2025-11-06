using Godot;

namespace Lastdew
{
	public class PcStateMovement : PcState
	{
		private const float RECALCULATION_DISTANCE_SQUARED = 0.25f;
		
		// TODO: Get this info from pc stats eventually? Or just have everyone move the same?
		private float MaxSpeed { get; set; } = 7f;
		private float Acceleration { get; set; } = 50f;
		private Vector3 LastTargetPosition { get; set; }
	
		public PcStateMovement(PlayerCharacter pc) : base(pc)
		{
			// pc.NavigationAgent.Connect(
			// 	NavigationAgent3D.SignalName.VelocityComputed,
			// 	Callable.From((Vector3 safeVelocity) => Move(safeVelocity)));
		}
	
		public override void PhysicsProcessUnselected(float delta)
		{
			MovementProcess(delta);
		}
	
		public override void PhysicsProcessSelected(float delta)
		{
			MovementProcess(delta);
		}
	
		public override void EnterState()
		{
			if (Pc.MovementTarget.Target is Enemy enemy)
			{
				Vector3 attackPosition = AttackPosition(enemy.GlobalPosition);
				Pc.NavigationAgent.TargetPosition = attackPosition;
				LastTargetPosition = attackPosition;
			}
			else
			{
				Pc.NavigationAgent.TargetPosition = Pc.MovementTarget.TargetPosition;
			}
			//this.PrintDebug($"Move target position: {Context.NavigationAgent.TargetPosition}");
		}
	
		public override void ExitState()
		{
			Pc.PcAnimationTree.Set(BlendAmountPath, 0);
		}
		public override void ProcessUnselected(float delta) {}
		public override void ProcessSelected(float delta) {}
	
		private void MovementProcess(float delta)
		{
			if (WithinRangeOfTarget())
			{
				PcStateNames stateName;
				switch (Pc.MovementTarget.Target)
				{
					case null:
						stateName = PcStateNames.IDLE;
						break;
					case LootContainer:
						stateName = PcStateNames.LOOTING;
						break;
					case Enemy:
						stateName = PcStateNames.COMBAT;
						break;
					default:
						GD.PushWarning("MovementTarget.Target's type isn't null, LootContainer, or Enemy");
						stateName = PcStateNames.IDLE;
						break;
				}
				ChangeState(stateName);
				return;
			}
			Animate();
			RecalculateTargetPositionIfTargetMovedEnough();
			MoveAndRotate(delta);
		}

		private bool WithinRangeOfTarget()
		{
			//this.PrintDebug($"Distance: {Pc.GlobalPosition.DistanceTo(Pc.MovementTarget.Target.GlobalPosition)}");
			if (Pc.MovementTarget.Target is Enemy enemy)
			{
				return Pc.GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition) < AttackRadius * AttackRadius;
			}

			return Pc.GlobalPosition.DistanceSquaredTo(Pc.MovementTarget.TargetPosition) < AttackRadius * AttackRadius;
		}

		private void Animate()
		{
			float blendAmount = Mathf.Clamp(Pc.Velocity.Length() / MaxSpeed, 0, 1);
			Pc.PcAnimationTree.Set(BlendAmountPath, blendAmount);
		}
		
		private void RecalculateTargetPositionIfTargetMovedEnough()
		{
			if (Pc.MovementTarget.Target is Enemy enemy)
			{
				Vector3 attackPosition = AttackPosition(enemy.GlobalPosition);
				if (attackPosition.DistanceSquaredTo(LastTargetPosition) > RECALCULATION_DISTANCE_SQUARED)
				{
					Pc.NavigationAgent.TargetPosition = attackPosition;
					LastTargetPosition = attackPosition;
				}
			}
		}
	
		private void MoveAndRotate(float delta)
		{
			Vector3 nextPosition = Pc.NavigationAgent.GetNextPathPosition();
			Pc.RotateToward(nextPosition, TurnSpeed * delta);
			
			Vector3 direction = (nextPosition - Pc.GlobalPosition).Normalized();
			Accelerate(direction * MaxSpeed, Acceleration * delta);
		}

		private void Accelerate(Vector3 targetVelocity, float accelerationAmount)
		{
			Pc.NavigationAgent.Velocity = Pc.Velocity.MoveToward(targetVelocity, accelerationAmount);
			// Uncomment below for non nav-avoidance movement.
			 Pc.Velocity = Pc.Velocity.MoveToward(targetVelocity, accelerationAmount);
			 Pc.MoveAndSlide();
		}

		// private void Move(Vector3 velocity)
		// {
		// 	Pc.Velocity = velocity;
		// 	Pc.MoveAndSlide();
		// }
		
		/// <summary>
        /// TODO: Is this necessary or does nav agent take care of the "attack radius" somehow?
        /// </summary>
		private Vector3 AttackPosition(Vector3 enemyPosition)
		{
			Vector3 direction = (Pc.GlobalPosition - enemyPosition).Normalized();
			return enemyPosition + direction * AttackRadius;
		}
	}
}
