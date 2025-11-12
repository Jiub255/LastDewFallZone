using Godot;

namespace Lastdew
{
	public partial class CraftablesTester : Node
	{
		public override void _Ready()
		{
			base._Ready();

			Craftables craftables = Databases.Craftables;
			this.PrintDebug(
				$"Buildings: {craftables.Buildings.Count}, " +
				$"Materials: {craftables.CraftingMaterials.Count}, " +
				$"Equipment: {craftables.Equipments.Count}, " +
				$"Usable Items: {craftables.UsableItems.Count}");
		}
	}
}
