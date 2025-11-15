using System.Collections.Generic;
using Godot;

namespace Lastdew
{	
	public static class Extensions
	{
		private const int PADDING = 70;

		/// <summary>
		/// Prints the type and name of the object that sent message.
		/// </summary>
		public static void PrintDebug(this object obj, string message)
		{
			switch (obj)
			{
				case null:
					return;
				case Resource resource:
				{
					string type = resource.GetType().Name;
					if (resource.GetType().BaseType != typeof(RefCounted))
					{
						type += $" : {resource.GetType().BaseType?.Name}";
					}
					GD.Print($"Resource    |    {type}".PadRight(PADDING)
					         + $" |    {message}");
					break;
				}
				case RefCounted refCounted:
				{
					string type = refCounted.GetType().Name;
					GD.Print($"RefCounted  |    {type}".PadRight(PADDING)
					         + $" |    {message}");
					break;
				}
				case Node node:
				{
					string name = node.Name;
					if (node.GetType().BaseType != typeof(GodotObject))
					{
						name += $" : {node.GetType().BaseType?.Name}";
					}
					GD.Print($"Node        |    {name}".PadRight(PADDING)
					         + $" |    {message}");
					break;
				}
				default:
				{
					string type = obj.GetType().Name;
					if (obj.GetType().BaseType != null)
					{
						type += $" : {obj.GetType().BaseType?.Name}";
					}
					GD.Print($"C# Object   |    {type}".PadRight(PADDING)
					         + $" |    {message}");
					break;
				}
			}
		}
		
		public static void RotateToward(this Node3D node3D, Vector3 lookTarget, float turnAmount)
		{
			lookTarget = new Vector3(
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

		public static string CommaFormatList(this string[] words)
		{
			System.Array.Reverse(words);
			string formatted = "";
			string oxfordComma = words.Length > 2 ? "," : "";
			for (int i = 0; i < words.Length; i++)
			{
				formatted = i switch
				{
					0 => words[i],
					1 => words[i] + oxfordComma + " and " + formatted,
					_ => words[i] + ", " + formatted
				};
			}
			return formatted;
		}
		
		public static bool IsLeftClick(this InputEvent @event)
		{
			return @event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true };
		}
		
		public static long GetUid(this Resource resource)
		{
			return ResourceLoader.GetResourceUid(resource?.ResourcePath);
		}
		
		public static Node[] GetChildrenRecursive(this Node node)
		{
			List<Node> nodes = [];
			Godot.Collections.Array<Node> children = node.GetChildren();
			nodes.Add(node);
			if (children.Count == 0)
			{
				return [.. nodes];
			}
			foreach (Node child in children)
			{
				nodes.AddRange(GetChildrenRecursive(child));
			}
			return [.. nodes];
		}
		
		public static string ToSnakeCase(this string text)
		{
			text = text.ToLower();
			string[] words = text.Split(" ");
			return words.Join("_");
		}
	}
}
