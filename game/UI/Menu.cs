using Godot;

namespace Lastdew
{
	public abstract partial class Menu : CanvasLayer
	{
		public virtual void Open()
		{
			Show();
		}
		public virtual void Close()
		{
			Hide();
		}
	}
}
