using Godot;

namespace Lastdew
{
	public class PcStateMovement : PcState
	{
		private const float RECALCULATION_DISTANCE_SQUARED = 0.25f;
		
		public MovementTarget MovementTarget { get; private set; }
	
		// TODO: Get this info from pc stats eventually? Or just have everyone move the same?
		private float MaxSpeed { get; set; } = 7f;
		private float Acceleration { get; set; } = 50f;
		private Vector3 LastTargetPosition { get; set; }
	
		public PcStateMovement(PcStateContext context) : base(context)
		{
			context.NavigationAgent.Connect(
				NavigationAgent3D.SignalName.VelocityComputed,
				Callable.From((Vector3 safeVelocity) => SetSafeVelocity(safeVelocity)));
		}
	
		public override void PhysicsProcessUnselected(float delta)
		{
			MovementProcess(delta);
		}
	
		public override void PhysicsProcessSelected(float delta)
		{
			MovementProcess(delta);
		}
	
		public override void EnterState(MovementTarget target)
		{
			MovementTarget = target;
			if (target.Target is Enemy)
			{
				Vector3 attackPosition = AttackPosition(target.Target.GlobalPosition);
				Context.NavigationAgent.TargetPosition = attackPosition;
				LastTargetPosition = attackPosition;
			}
			else
			{
				Context.NavigationAgent.TargetPosition = target.TargetPosition;
			}
			//this.PrintDebug($"Move target position: {Context.NavigationAgent.TargetPosition}");
		}
	
		public override void ExitState()
		{
			Context.PcAnimationTree.Set(BlendAmountPath, 0);
		}
		public override void ProcessUnselected(float delta) {}
		public override void ProcessSelected(float delta) {}
	
		private void MovementProcess(float delta)
		{
			if (DestinationReached())
			{
				PcStateNames stateName;
				switch (MovementTarget.Target)
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
				ChangeState(stateName, MovementTarget);
				if (stateName != PcStateNames.IDLE)
				{
					// TODO: Face PC toward target. Just instantaneously for now, can do a tween/lerp later.
				}
				return;
			}
			Animate();
			RecalculateTargetPositionIfTargetMovedEnough();
			MoveAndRotate(delta);
		}
	
		private bool DestinationReached()
		{
			bool navFinished = Context.NavigationAgent.IsNavigationFinished();
			return navFinished;
		}
	
		private void Animate()
		{
			float blendAmount = Mathf.Clamp(Context.Speed / MaxSpeed, 0, 1);
			Context.PcAnimationTree.Set(BlendAmountPath, blendAmount);
		}
		
		private void RecalculateTargetPositionIfTargetMovedEnough()
		{
			if (MovementTarget.Target is Enemy enemy)
			{
				Vector3 attackPosition = AttackPosition(enemy.GlobalPosition);
				if (attackPosition.DistanceSquaredTo(LastTargetPosition) > RECALCULATION_DISTANCE_SQUARED)
				{
					Context.NavigationAgent.TargetPosition = attackPosition;
					LastTargetPosition = attackPosition;
				}
			}
		}
	
		private void MoveAndRotate(float delta)
		{
			Vector3 nextPosition = Context.NavigationAgent.GetNextPathPosition();
			Context.RotateToward(nextPosition, TurnSpeed * delta);
			
			Vector3 direction = (nextPosition - Context.GlobalPosition).Normalized();
			Context.Accelerate(direction * MaxSpeed, Acceleration * delta);
		}
	
		private void SetSafeVelocity(Vector3 safeVelocity)
		{
			Context.Move(safeVelocity);
		}
		
		private Vector3 AttackPosition(Vector3 enemyPosition)
		{
			Vector3 direction = (Context.GlobalPosition - enemyPosition).Normalized();
			return enemyPosition + direction * AttackRadius;
		}
	}
}
