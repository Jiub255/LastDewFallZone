using Godot;

namespace Lastdew
{	
	public partial class GameMenu : Menu
	{
		private CharacterTab CharacterTab { get; set; }
		private TeamData TeamData { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			CharacterTab = GetNode<CharacterTab>("%Character");
		}
		
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			TeamData = teamData;
			CharacterTab.Initialize(teamData, inventoryManager);
		}
		
		public override void Open()
		{
			base.Open();
			
			CharacterTab.PopulateInventoryUI();
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
