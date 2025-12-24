using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class BuildMenu : Menu
	{
		[Signal]
		public delegate void OnBuildEventHandler(Building3D building);
		
		private Building CurrentBuilding { get; set; }
		private TeamData TeamData { get; set; }
		private static PackedScene ButtonScene => GD.Load<PackedScene>(Uids.BUILDING_BUTTON);
		private GridContainer ButtonParent { get; set; }
		private List<BuildingButton> Buttons { get; } = [];
		private Label Description { get; set; }
		private Camera Camera { get; set; }
		private SfxButton BuildButton { get; set; }

		public override void Open()
		{
			base.Open();
			
			Setup();
			Camera.ProcessMode = ProcessModeEnum.Always;
			Camera.BuildMode = true;
		}

		public override void Close()
		{
			base.Close();

			Camera.ProcessMode = ProcessModeEnum.Inherit;
			Camera.BuildMode = false;
		}

		public void ConnectSignals(InventoryManager inventory)
		{
			BuildButton = GetNode<SfxButton>("%Build");
			BuildButton.Connect(
				BaseButton.SignalName.Pressed,
				Callable.From(BuildInstance));
			inventory.Connect(
				InventoryManager.SignalName.OnInventoryChanged,
				Callable.From(Setup));
		}
		
		public void Initialize(TeamData teamData, Camera camera)
		{
			TeamData = teamData;
			Camera = camera;
			ButtonParent = GetNode<GridContainer>("%Buttons");
			Description = GetNode<Label>("%Description");
			
		}

		public void Setup()
		{
			BuildButton.Disabled = true;
			
			ClearButtons();
			List<Building> unbuildables = [];
			foreach (Building building in Databases.Craftables.Buildings.Values)
			{
				if (!building.HasRequiredBuildings(TeamData.Inventory.Buildings) ||
				    !building.HasStatsToCraft(TeamData.MaximumStats))
				{
					continue;
				}
				
				if (building.HasEnoughMaterialsToBuild(TeamData.Inventory))
				{
					SetupButton(building);
				}
				else
				{
					unbuildables.Add(building);
				}
			}

			foreach (Building building in unbuildables)
			{
				SetupButton(building, true);
			}
		}

		private void ClearButtons()
		{
			foreach (BuildingButton button in Buttons)
			{
				button.QueueFree();
			}
			Buttons.Clear();
		}

		private void SetupButton(Building building, bool gray = false)
		{
			BuildingButton button = (BuildingButton)ButtonScene.Instantiate();
			button.Setup(building);
			ButtonParent.AddChildDeferred(button);
			button.Connect(
				BuildingButton.SignalName.OnPressed,
				Callable.From<Building>(SetBuilding));
			button.SetColor(gray);
			Buttons.Add(button);
		}
		
		private void SetBuilding(Building building)
		{
			CurrentBuilding = building;
			Description.Text = FormatDescription(building);
			BuildButton.Disabled = !building.HasEnoughMaterialsToBuild(TeamData.Inventory);
		}

		private void BuildInstance()
		{
			Camera.ClickHandlerBuild.Building3D?.QueueFree();
			
			if (CurrentBuilding == null)
			{
				return;
			}
			
			PackedScene buildingScene = GD.Load<PackedScene>(CurrentBuilding.SceneUid);
			Building3D building3D = (Building3D)buildingScene.Instantiate();
			Camera.ClickHandlerBuild.Building3D = building3D;
			Camera.ClickHandlerBuild.Building = CurrentBuilding;
			EmitSignal(SignalName.OnBuild, building3D);
		}

		private static string FormatDescription(Building building)
		{
			string text = $"{building.Description}\n\nRecipe Costs:";
			foreach ((long uid, int amount) in building.RecipeCosts)
			{
				string name = Databases.Craftables.CraftingMaterials[uid].Name;
				text += $"\n{amount} {name}";
			}
			return text;
		}
	}
}
