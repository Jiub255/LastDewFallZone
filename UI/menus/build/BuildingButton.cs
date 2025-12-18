using Godot;

namespace Lastdew
{
	public partial class BuildingButton : SfxButton
	{
		[Signal]
		public delegate void OnPressedEventHandler(Building building); 
		
		private Building Building { get; set; }

		public void Setup(Building building)
		{
			Building = building;
			Icon = building.Icon;
			TooltipText = building.Name;
			Connect(
				BaseButton.SignalName.Pressed,
				Callable.From(() => EmitSignal(SignalName.OnPressed, Building)));
		}
	}
}
