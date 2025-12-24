using System;

namespace Lastdew
{
	public partial class CraftableButton : SfxButton
	{
		public event Action<Item> OnPressed;
			
		public Item Item { get; private set; }
		
		public void Initialize(Item item)
		{
			Item = item;
			Icon = item.Icon;

			Pressed += InvokeOnPressed;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			Pressed -= InvokeOnPressed;
		}

		private void InvokeOnPressed()
		{
			OnPressed?.Invoke(Item);
		}
	}
}
