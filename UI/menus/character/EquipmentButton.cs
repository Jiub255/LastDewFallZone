using Godot;

namespace Lastdew
{
	public partial class EquipmentButton : SfxButton
	{
		[Signal]
		public delegate void OnPressedEventHandler(EquipmentType equipmentType);
		
		private Equipment _equipment;

		public Equipment Equipment
		{
			get => _equipment;
			set
			{
				_equipment = value;
				Icon = _equipment?.Icon;
			}
		}
		public EquipmentType Type {	get; set; }

		public override void _Ready()
		{
			base._Ready();

			Connect(BaseButton.SignalName.Pressed,
				Callable.From(() => EmitSignal(SignalName.OnPressed, (int)Type)));
		}
	}
}
