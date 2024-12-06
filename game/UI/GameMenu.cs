using Godot;

namespace Lastdew
{	
	public partial class GameMenu : CanvasLayer
	{
		private CharacterTab Character { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			Character = GetNode<CharacterTab>("%Character");
		}
		
		public void Initialize(InventoryManager inventoryManager)
		{
			Character.Initialize(inventoryManager);
		}
	
		public override void _Input(InputEvent @event)
		{
			base._Input(@event);
			
			if (@event.IsActionPressed(InputNames.GAME_MENU))
			{
				if (!Visible)
				{
					Character.PopulateInventoryUI();
					Show();
				}
				else
				{
					Hide();
				}
				//Visible = !Visible;
			}
			if (@event.IsActionPressed(InputNames.EXIT_MENU))
			{
				Hide();
			}
		}
	}
}
