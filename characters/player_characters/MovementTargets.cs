using System.Linq;
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	public class MovementTargets : List<MovementTarget>
	{
		/// <returns>true if movementTarget.Target is a LootContainer, and the MovementTargets list
		/// already contains other loot container MovementTargets.</returns>
		public bool Add(MovementTarget movementTarget, PlayerCharacter pc)
		{
			bool lootBeingAddedToLoot = false;
			if (movementTarget.Target is LootContainer)
			{
				if (Count > 0)
				{
					if (this.Any(x => x.Target is not LootContainer))
					{
						Clear();
					}
					else
					{
						lootBeingAddedToLoot = true;
					}
				}
				
				base.Add(movementTarget);
				SortByDistance(pc);	
			}
			else
			{
				Clear();
				base.Add(movementTarget);
			}
			return lootBeingAddedToLoot;
		}
		
		// TODO: Add "Remove" method, triggered by clicking on the loot container HUD. Sims-style.
		public void Remove(MovementTarget movementTarget, PlayerCharacter pc)
		{
			
		}

		public void SortByDistance(PlayerCharacter pc)
		{
			Sort(comparison: (MovementTarget x, MovementTarget y)
				=> DistanceFromPc(x, pc).CompareTo(DistanceFromPc(y, pc)));
		}

		private static float DistanceFromPc(MovementTarget target, PlayerCharacter pc)
		{
			if (target.Target is not LootContainer lootContainer)
			{
				GD.PushError($"Should only be called for loot containers.");
				return float.MaxValue;
			}

			Vector3[] path = GetNavigationPath(pc, lootContainer.LootingPosition);
			return path.Sum(segment => segment.Length());
		}
		
		private static Vector3[] GetNavigationPath(PlayerCharacter pc, Vector3 targetPosition)
		{
			if (!pc.IsInsideTree())
			{
				return [];
			}

			Rid defaultMapRid = pc.GetWorld3D().NavigationMap;
			Vector3[] path = NavigationServer3D.MapGetPath(
				defaultMapRid,
				pc.GlobalPosition,
				targetPosition,
				true
			);
			return path;
		}
	}
}
