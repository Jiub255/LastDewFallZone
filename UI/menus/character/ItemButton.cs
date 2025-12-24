using System;
using Godot;

namespace Lastdew
{	
	public partial class ItemButton : SfxButton
	{
		public event Action<ItemButton> OnPressed;
		
		public Item Item { get; private set; }
		public int Amount { get; set; }
		
		private Label AmountLabel { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			AmountLabel = GetNode<Label>("%Label");
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			Pressed -= InvokeOnPressed;
		}

		// CallDeferred from CharacterMenu.SetupButton().
		public void Initialize(Item item, int amount)
		{
			Item = item;
			Amount = amount;
			Icon = item.Icon;
			AmountLabel.Text = amount.ToString();
			Pressed += InvokeOnPressed;
		}

		private void InvokeOnPressed()
		{
			OnPressed?.Invoke(this);
		}
	}
}
