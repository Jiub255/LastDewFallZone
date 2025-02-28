using Godot;

namespace Lastdew
{
	public partial class CraftablesTester : Node
	{
		private Craftables Craftables { get; set; } = GD.Load<Craftables>(UIDs.CRAFTABLES);

		public override void _Ready()
		{
			base._Ready();

			this.PrintDebug(
				$"Buildings: {Craftables.Buildings.Count}, " +
				$"Materials: {Craftables.Materials.Count}, " +
				$"Equipment: {Craftables.Equipment.Count}, " +
				$"Usable Items: {Craftables.UsableItems.Count}");
		}
	}
}
