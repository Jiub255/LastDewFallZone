using Godot;

namespace Lastdew
{
	public class PcStateContext
	{
		public NavigationAgent3D NavigationAgent { get; }
		public PcAnimationTree PcAnimationTree { get; }
		public Vector3 Position
		{
			get => PC.Position;
		}
		public Vector3 GlobalPosition
		{
			get => PC.GlobalPosition;
		}
		public float Speed
		{
			get => PC.Velocity.Length();
		}
		public InventoryManager InventoryManager { get; }
		
		private PlayerCharacter PC { get; }
		
		public PcStateContext(PlayerCharacter pc, InventoryManager inventoryManager)
		{
			PC = pc;
			InventoryManager = inventoryManager;
			
			NavigationAgent = pc.GetNode<NavigationAgent3D>("%NavigationAgent3D");
			PcAnimationTree = pc.GetNode<PcAnimationTree>("%AnimationTree");
			
			PcAnimationTree.Connect(
				AnimationTree.SignalName.AnimationFinished,
				Callable.From((string animationName) => ResetAnimationParameter(animationName)));
		}
		
		public void Move(Vector3 velocity)
		{
			PC.Velocity = velocity;
			PC.MoveAndSlide();
		}
		
		public void RotateToward(Vector3 nextPosition, float turnAmount)
		{
			PC.RotateToward(nextPosition, turnAmount);
		}
		
		public void Accelerate(Vector3 targetVelocity, float accelerationAmount)
		{
			NavigationAgent.Velocity = PC.Velocity.MoveToward(targetVelocity, accelerationAmount);
		}
		
		private void ResetAnimationParameter(string animationName)
		{
			if (animationName == "HitRecieve") // Match the "GetHit" animation's name.
			{
				PcAnimationTree.Set("parameters/conditions/GettingHit", false);
			}
			else if (animationName == "Punch_Right")
			{
				PcAnimationTree.Set("parameters/conditions/Attacking", false);
			}
		}
	}
}
