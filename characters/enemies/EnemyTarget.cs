using Godot;
using System;

namespace Lastdew
{
	public class EnemyTarget(PlayerCharacter pc, Vector3 combatDirection)
	{
		public PlayerCharacter PC { get; set; } = pc;
	    public Vector3 CombatDirection { get; set; } = combatDirection;
	}
}
