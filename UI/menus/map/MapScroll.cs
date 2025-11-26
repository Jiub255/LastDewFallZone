using Godot;

namespace Lastdew
{
	public partial class MapScroll : ScrollContainer
	{
		private bool ButtonDown { get; set; }

		public override void _Process(double delta)
		{
			base._Process(delta);
			
			if (ButtonDown && Input.IsActionJustReleased(InputNames.SELECT))
			{
				ButtonDown = false;
			}
		}

		public override void _GuiInput(InputEvent @event)
		{
			base._GuiInput(@event);
			
			if (!ButtonDown && @event.IsLeftClick())
			{
				ButtonDown = true;
			}
			
			if (ButtonDown && @event is InputEventMouseMotion mouseMotion)
			{
				ScrollHorizontal -= (int)mouseMotion.Relative.X;
				ScrollVertical -= (int)mouseMotion.Relative.Y;
			}
		}
	}
}
