using Godot;
using System;

namespace Lastdew
{	
	public static class Extensions
	{
		private static readonly int PADDING = 70;
		
		/// <summary>
		/// Prints the type and name of the object that sent message.
		/// </summary>
		public static void PrintDebug(this object obj, string message)
		{
			if (obj is Resource resource)
			{
				string type = resource.GetType().Name;
				if (resource.GetType().BaseType != typeof(RefCounted))
				{
					type += $" : {resource.GetType().BaseType.Name}";
				}
				GD.Print($"Resource    |    {type}".PadRight(PADDING)
					+ $" |    {message}");
			}
			else if (obj is RefCounted refCounted)
			{
				string type = refCounted.GetType().Name;
				GD.Print($"RefCounted  |    {type}".PadRight(PADDING)
					+ $" |    {message}");
			}
			else if (obj is Node node)
			{
				string name = node.Name;
				if (node.GetType().BaseType != typeof(GodotObject))
				{
					name += $" : {node.GetType().BaseType.Name}";
				}
				GD.Print($"Node        |    {name}".PadRight(PADDING)
					+ $" |    {message}");
			}
			else
			{
				string type = obj.GetType().Name;
				if (obj.GetType().BaseType != null)
				{
					type += $" : {obj.GetType().BaseType.Name}";
				}
				GD.Print($"C# Object   |    {type}".PadRight(PADDING)
					+ $" |    {message}");
			}
		}
		
		// TODO: Probably need to fix this, haven't tested it yet.
		public static void RotateToward(this Node3D node3D, Vector3 lookTarget, float turnAmount)
		{
			lookTarget = new(
				lookTarget.X,
				node3D.GlobalPosition.Y,
				lookTarget.Z);
			Vector3 directionToTarget = (lookTarget - node3D.GlobalPosition).Normalized();
			Vector3 forward = node3D.GlobalTransform.Basis.Z.Normalized();
			
			float angleToTarget = Mathf.RadToDeg(forward.AngleTo(directionToTarget));
			
			// Check to make sure not rotating the long way around.
			// Cross product is perpendicular to forward and directionToTarget, so it points up or down.
			Vector3 cross = forward.Cross(directionToTarget);
			// If it points down (dot product < 0), then the angle's sign needs to flip. 
			if (cross.Dot(Vector3.Up) < 0)
			{
				angleToTarget = -angleToTarget;
			}
			
			float rotationAmount = Mathf.Min(Mathf.Abs(angleToTarget), turnAmount);
			node3D.RotateObjectLocal(Vector3.Up, Mathf.DegToRad(rotationAmount * Mathf.Sign(angleToTarget)));
		}
	}
}
