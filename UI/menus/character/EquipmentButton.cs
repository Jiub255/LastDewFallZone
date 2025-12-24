using System;

namespace Lastdew
{
	public partial class EquipmentButton : SfxButton
	{
		public event Action<EquipmentType> OnPressed;
		
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

			Pressed += InvokeOnPressed;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			Pressed -= InvokeOnPressed;
		}

		private void InvokeOnPressed()
		{
			OnPressed?.Invoke(Type);
		}
	}
}
