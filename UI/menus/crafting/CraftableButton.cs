using Godot;
using System;

namespace Lastdew
{
	public partial class CraftableButton : SfxButton
	{
		[Signal]
		public delegate void OnPressedEventHandler(Item item);
			
		public Item Item { get; private set; }
		
		public void Initialize(Item item)
		{
			Item = item;
			Icon = item.Icon;
			Connect(
				BaseButton.SignalName.Pressed,
				Callable.From(() => EmitSignal(SignalName.OnPressed, Item)));
		}
	}
}
