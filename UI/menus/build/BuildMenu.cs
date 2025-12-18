using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class BuildMenu : Menu
	{
		[Signal]
		public delegate void OnBuildEventHandler(Building3D building);
		
		private Building CurrentBuilding { get; set; }
		private InventoryManager Inventory { get; set; }
		private static PackedScene ButtonScene => GD.Load<PackedScene>(Uids.BUILDING_BUTTON);
		private GridContainer Buttons { get; set; }
		private Label Description { get; set; }
		private Camera Camera { get; set; }
		private SfxButton BuildButton { get; set; }
		private List<BuildingSaveData> Buildings { get; set; }

		public override void _ExitTree()
		{
			base._ExitTree();
			
			Inventory.OnInventoryChanged -= Setup;
		}

		public override void Open()
		{
			base.Open();
			
			Setup();
			Camera.ProcessMode = ProcessModeEnum.Always;
			Camera.ClickHandler.BuildMode = true;
		}

		public override void Close()
		{
			base.Close();

			Camera.ProcessMode = ProcessModeEnum.Inherit;
			Camera.ClickHandler.BuildMode = false;
		}
		
		public void Initialize(InventoryManager inventory, Camera camera, List<BuildingSaveData> buildings)
		{
			Inventory = inventory;
			Camera = camera;
			Buildings = buildings;
			Buttons = GetNode<GridContainer>("%Buttons");
			Description = GetNode<Label>("%Description");
			BuildButton = GetNode<SfxButton>("%Build");
			
			
			// Hacky fix to double subscribing/connecting.
			// TODO: Separate initialize (first time only) from setup (each time) on UiManager.
			// if (!BuildButton.IsConnected(
			// 	    BaseButton.SignalName.Pressed,
			// 	    Callable.From(BuildInstance)))
			
			
			{
				BuildButton.Connect(
					BaseButton.SignalName.Pressed,
					Callable.From(BuildInstance));
				Inventory.OnInventoryChanged += Setup;
			}
		}

		public void Setup()
		{
			// TODO: Not 100% sure this should be here.
			BuildButton.Disabled = true;
			
			ClearButtons();
			List<Building> unbuildables = [];
			foreach (Building building in Databases.Craftables.Buildings.Values)
			{
				// TODO: Or just do early continue with HasRequiredBuildings() and not show them at all?
				
				//if (!building.HasRequiredBuildings(Buildings)) continue;
				if (building.HasEnoughMaterialsToBuild(Inventory) && 
				    building.HasRequiredBuildings(Buildings))
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
			foreach (Node child in Buttons.GetChildren())
			{
				if (child is BuildingButton button)
				{
					button.QueueFree();
				}
			}
		}

		private void SetupButton(Building building, bool gray = false)
		{
			BuildingButton button = (BuildingButton)ButtonScene.Instantiate();
			button.Setup(building);
			Buttons.CallDeferred(Node.MethodName.AddChild, button);
			button.Connect(
				BuildingButton.SignalName.OnPressed,
				Callable.From<Building>(SetBuilding));
			button.SetColor(gray);
		}
		
		private void SetBuilding(Building building)
		{
			CurrentBuilding = building;
			Description.Text = FormatDescription(building);
			BuildButton.Disabled = !building.HasRequiredBuildings(Buildings) ||
			                       !building.HasEnoughMaterialsToBuild(Inventory);
		}

		private void BuildInstance()
		{
			if (CurrentBuilding == null)
			{
				return;
			}

			PackedScene buildingScene = GD.Load<PackedScene>(CurrentBuilding.SceneUid);
			Building3D building3D = (Building3D)buildingScene.Instantiate();
			Camera.ClickHandler.Building3D = building3D;
			Camera.ClickHandler.Building = CurrentBuilding;
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
