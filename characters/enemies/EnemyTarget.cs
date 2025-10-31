using Godot;

namespace Lastdew
{
	public class EnemyTarget(PlayerCharacter pc, Vector3 combatDirection)
	{
		public PlayerCharacter PC { get; } = pc;
		public Vector3 CombatDirection { get; } = combatDirection;
	}
}
