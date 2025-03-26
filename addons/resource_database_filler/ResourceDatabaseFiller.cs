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
			Craftables craftables = GD.Load<Craftables>(UIDs.CRAFTABLES);
			if (craftables != null)
			{
				craftables.PopulateDictionaries();
			}
			else
			{
				GD.PrintErr("Craftables resource not found");
			}
		}

		private void OnPcDatasButtonPressed()
		{
			AllPcDatas allPcDatas = GD.Load<AllPcDatas>(UIDs.ALL_PC_DATAS);
			if (allPcDatas != null)
			{
				allPcDatas.PopulateDictionary();
			}
			else
			{
				GD.PrintErr("AllPcDatas resource not found");
			}
		}
	}
}
