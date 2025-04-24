using Godot;

namespace Lastdew
{
	public partial class CraftablesTester : Node
	{
		public override void _Ready()
		{
			base._Ready();

			Craftables craftables = Databases.CRAFTABLES;
			this.PrintDebug(
				$"Buildings: {craftables.Buildings.Count}, " +
				$"Materials: {craftables.Materials.Count}, " +
				$"Equipment: {craftables.Equipment.Count}, " +
				$"Usable Items: {craftables.UsableItems.Count}");
		}
	}
}
