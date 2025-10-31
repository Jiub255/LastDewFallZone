using Godot;

namespace Lastdew
{
	[Tool]
	public partial class ResourceDatabaseFiller : EditorPlugin
	{
		private Button CraftablesButton { get; set; }
		private Button PcDatasButton { get; set; }
		
		public override void _EnterTree()
		{
			// Add a custom button to the editor toolbar
			CraftablesButton = new Button
			{
				Text = "Populate Craftables"
			};
			CraftablesButton.Pressed += OnCraftablesButtonPressed;
			AddControlToContainer(CustomControlContainer.Toolbar, CraftablesButton);
			
			PcDatasButton = new Button
			{
				Text = "Populate PcDatas"
			};
			PcDatasButton.Pressed += OnPcDatasButtonPressed;
			AddControlToContainer(CustomControlContainer.Toolbar, PcDatasButton);
		}

		public override void _ExitTree()
		{
			CraftablesButton.Pressed -= OnCraftablesButtonPressed;
			RemoveControlFromContainer(CustomControlContainer.Toolbar, CraftablesButton);
			
			PcDatasButton.Pressed -= OnPcDatasButtonPressed;
			RemoveControlFromContainer(CustomControlContainer.Toolbar, PcDatasButton);
		}

		private void OnCraftablesButtonPressed()
		{
			Databases.Craftables.PopulateDictionaries();
		}

		private void OnPcDatasButtonPressed()
		{
			Databases.PcDatas.PopulateDictionary();
		}
	}
}
