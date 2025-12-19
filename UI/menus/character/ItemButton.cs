using System;
using Godot;

namespace Lastdew
{	
	public partial class ItemButton : SfxButton
	{
		[Signal]
		public delegate void OnPressedEventHandler(ItemButton itemButton);
		
		public Item Item { get; private set; }
		public int Amount { get; set; }
		
		private Label AmountLabel { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			AmountLabel = GetNode<Label>("%Label");
		}
	
		// CallDeferred from CharacterMenu.SetupButton().
		public void Initialize(Item item, int amount)
		{
			Item = item;
			Amount = amount;
			Icon = item.Icon;
			AmountLabel.Text = amount.ToString();
			Connect(BaseButton.SignalName.Pressed, Callable.From(() => EmitSignal(SignalName.OnPressed, this)));
		}
	}
}
