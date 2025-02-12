using Godot;
using System;

namespace Lastdew
{
	public partial class CraftableButton : Button
	{
		public event Action<Item> OnPressed;
			
		public Item Item { get; private set; }
	
		public override void _ExitTree()
		{
			base._ExitTree();
			
			Pressed -= PressButton;
		}
	
		public void Initialize(Item item)
		{
			Item = item;
			Icon = item.Icon;
			Pressed += PressButton;
		}
		
		private void PressButton()
		{
			OnPressed?.Invoke(Item);
		}
	}
}
