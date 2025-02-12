using System;
using Godot;

namespace Lastdew
{
	public partial class EquipmentButton : Button
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
			
			Pressed += InvokeEvent;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			Pressed -= InvokeEvent;
		}

		private void InvokeEvent()
		{
			OnPressed?.Invoke(Type);
		}
	}
}
