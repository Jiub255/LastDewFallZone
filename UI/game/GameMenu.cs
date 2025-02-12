using Godot;

namespace Lastdew
{	
	public partial class GameMenu : Menu
	{
		private CharacterTab CharacterTab { get; set; }
		private CraftingTab CraftingTab { get; set; }
		private TeamData TeamData { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			CharacterTab = GetNode<CharacterTab>("%Character");
			CraftingTab = GetNode<CraftingTab>("%Crafting");
		}
		
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			TeamData = teamData;
			CharacterTab.Initialize(teamData, inventoryManager);
			CraftingTab.Initialize(inventoryManager);
		}
		
		public override void Open()
		{
			base.Open();
			
			CharacterTab.RefreshDisplay();
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
