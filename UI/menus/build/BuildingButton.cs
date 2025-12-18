using Godot;

namespace Lastdew
{
	public partial class BuildingButton : SfxButton
	{
		[Signal]
		public delegate void OnPressedEventHandler(Building building); 
		
		private Building Building { get; set; }
		private static Color Gray => new Color(0.5f, 0.5f, 0.5f, 0.5f);

		public void Setup(Building building)
		{
			Building = building;
			Icon = building.Icon;
			TooltipText = building.Name;
			Connect(
				BaseButton.SignalName.Pressed,
				Callable.From(() => EmitSignal(SignalName.OnPressed, Building)));
		}

		public void SetColor(bool gray)
		{
			Modulate = gray ? Gray : Colors.White;
		}
	}
}
