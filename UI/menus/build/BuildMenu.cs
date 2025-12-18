using Godot;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class BuildMenu : Menu
	{
		[Signal]
		public delegate void OnBuildEventHandler(Node3D building);
		
		private Building CurrentBuilding { get; set; }
		private InventoryManager Inventory { get; set; }
		private static PackedScene ButtonScene => GD.Load<PackedScene>(Uids.BUILDING_BUTTON);
		private GridContainer Buttons { get; set; }
		private Label Description { get; set; }
		private Camera Camera { get; set; }
		private SfxButton BuildButton { get; set; }
		private List<Building> Buildings { get; set; }
		
		public void Initialize(InventoryManager inventory, Camera camera, List<Building> buildings)
		{
			Inventory = inventory;
			Camera = camera;
			Buildings = buildings;
			Buttons = GetNode<GridContainer>("%Buttons");
			Description = GetNode<Label>("%Description");
			BuildButton = GetNode<SfxButton>("%Build");
			BuildButton.Connect(
				BaseButton.SignalName.Pressed,
				Callable.From(BuildInstance));
			BuildButton.Disabled = true;
			
			SetupButtons();
		}

		public override void Open()
		{
			base.Open();
			
			SetupButtons();
			Camera.ProcessMode = ProcessModeEnum.Always;
			Camera.ClickHandler.BuildMode = true;
		}

		public override void Close()
		{
			base.Close();

			Camera.ProcessMode = ProcessModeEnum.Inherit;
			Camera.ClickHandler.BuildMode = false;
		}

		private void SetupButtons()
		{
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
				// TODO: Make the unbuildable buttons look gray.
				SetupButton(building);
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

		private void SetupButton(Building building)
		{
			BuildingButton button = (BuildingButton)ButtonScene.Instantiate();
			button.Setup(building);
			Buttons.CallDeferred(Node.MethodName.AddChild, button);
			button.Connect(
				BuildingButton.SignalName.OnPressed,
				Callable.From<Building>(SetBuilding));
		}
		
		private void SetBuilding(Building building)
		{
			CurrentBuilding = building;
			Description.Text = FormatDescription(building);
			BuildButton.Disabled = !building.HasRequiredBuildings(Buildings);
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
