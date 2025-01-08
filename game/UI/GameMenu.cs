using Godot;

namespace Lastdew
{	
	public partial class GameMenu : CanvasLayer
	{
		private CharacterTab Character { get; set; }
		private TeamData TeamData { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			Character = GetNode<CharacterTab>("%Character");
		}
		
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			TeamData = teamData;
			Character.Initialize(teamData, inventoryManager);
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
					CloseMenu();
				}
				//Visible = !Visible;
			}
			if (@event.IsActionPressed(InputNames.EXIT_MENU))
			{
				CloseMenu();
			}
		}
		
		private void CloseMenu()
		{
			if (TeamData.SelectedIndex != null)
			{
				TeamData.MenuSelectedIndex = (int)TeamData.SelectedIndex;
			}
			Hide();
		}
	}
}
