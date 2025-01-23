using Godot;

namespace Lastdew
{	
	public partial class GameMenu : Menu
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
		
		public override void Open()
		{
			base.Open();
			
			Character.PopulateInventoryUI();
		}
		
		public override void Close()
		{
			base.Close();
			
			if (TeamData.SelectedIndex != null)
			{
				TeamData.MenuSelectedIndex = (int)TeamData.SelectedIndex;
			}
		}
	}
}
