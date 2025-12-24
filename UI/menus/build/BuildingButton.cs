using System;
using Godot;

namespace Lastdew
{
	public partial class BuildingButton : SfxButton
	{
		public event Action<Building> OnPressed;
		
		private Building Building { get; set; }
		private static Color Gray => new Color(0.5f, 0.5f, 0.5f, 0.5f);

		public override void _ExitTree()
		{
			base._ExitTree();

			Pressed -= InvokeOnPressed;
		}

		public void Setup(Building building)
		{
			Building = building;
			Icon = building.Icon;
			TooltipText = building.Name;
			Pressed += InvokeOnPressed;
		}

		public void SetColor(bool gray)
		{
			Modulate = gray ? Gray : Colors.White;
		}

		private void InvokeOnPressed()
		{
			OnPressed?.Invoke(Building);
		}
	}
}
