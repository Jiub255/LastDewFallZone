using Godot;

public partial class Enemy : CharacterBody3D
{
	// TODO: Start with simple test class. Don't bother with state machine and health component and stuff yet.
	private const float RECALCULATION_DISTANCE_SQUARED = 4.0f;
	private const float ATTACK_RANGE = 10.0f;
	
	private int Health { get; set; }
	private int Attack { get; set;}
	private int Speed { get; set;}
	private PlayerCharacter Target { get; set; }
	private NavigationAgent3D NavigationAgent { get; set; }
	private Vector3 LastTargetPosition { get; set; }

	public override void _Ready()
	{
		base._Ready();
		
		NavigationAgent = GetNode<NavigationAgent3D>("%NavigationAgent3D");
	}
	
	public override void _Process(double delta)
	{
		base._Process(delta);
		
		
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
