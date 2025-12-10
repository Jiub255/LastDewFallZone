using Godot;

namespace Lastdew
{	
	public partial class GameMenu : Menu
	{
		private CharacterMenu CharacterMenu { get; set; }
		private CraftingMenu CraftingMenu { get; set; }
		private TeamData TeamData { get; set; }
	
		public override void _Ready()
		{
			base._Ready();
			
			CharacterMenu = GetNode<CharacterMenu>("%Character");
			CraftingMenu = GetNode<CraftingMenu>("%Crafting");
		}
		
		public void Initialize(TeamData teamData, InventoryManager inventoryManager)
		{
			TeamData = teamData;
			CharacterMenu.Initialize(teamData, inventoryManager);
			CraftingMenu.Initialize(inventoryManager);
		}
		
		public override void Open()
		{
			base.Open();
			
			CharacterMenu.RefreshDisplay();
		}
		
		public override void Close()
		{
			base.Close();

			if (TeamData?.SelectedIndex != null)
			{
				TeamData.MenuSelectedIndex = (int)TeamData.SelectedIndex;
			}
		}
	}
}
