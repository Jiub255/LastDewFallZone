#if TOOLS
using Godot;
using Godot.Collections;

namespace Lastdew
{
    /// <summary>
    /// TODO: Combine RecipeCostsEdit, RequiredBuildingsEdit, and StatsToCraftEdit into a base class.
    /// Probably replace IPropertyUI interface with the abstract base class.
    /// </summary>
	[Tool]
	public partial class RequiredBuildingsEdit : HBoxContainer, IPropertyUi
	{
        private HBoxContainer Parent { get; set; }
        private Button Add { get; set; }
        private PackedScene BuildingEditScene { get; } = GD.Load<PackedScene>(UIDs.BUILDING_EDIT);
        private Array<Building> Buildings
        {
            get
            {
                return GatherBuildings();
            }
        }

        public override void _Ready()
        {
            base._Ready();

            Parent = GetNode<HBoxContainer>("%Parent");
            Add = GetNode<Button>("%Add");

            Add.Pressed += NewBuildingEdit;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            Add.Pressed -= NewBuildingEdit;
        }
        
        public void Setup(Array<Building> buildings)
        {
            this.PrintDebug($"Setting up {buildings.Count} Building Edits");
            ClearBuildingEdits();
            foreach (Building building in buildings)
            {
                NewBuildingEdit(building);
            }
        }
        
        public void Save(Craftable craftable)
        {
            this.PrintDebug($"Saving {craftable.Name}");
            foreach (Building building in Buildings)
            {
                this.PrintDebug($"Building: {building?.Name}");
            }
            craftable.Set(Craftable.PropertyName._requiredBuildings, Buildings);
        }
        
        private Array<Building> GatherBuildings()
        {
            Array<Building> buildings;
            buildings = [];
            foreach (Node node in Parent.GetChildren())
            {
                if (node is BuildingEdit buildingEdit && buildingEdit.Building != null)
                {
                    buildings.Add(buildingEdit.Building);
                }
            }
            return buildings;
        }
        
        private void NewBuildingEdit(Building building)
        {
            int children = Parent.GetChildren().Count;
            if (children <= 3)
            {
                BuildingEdit buildingEdit = (BuildingEdit)BuildingEditScene.Instantiate();
                Parent.AddChild(buildingEdit);
                buildingEdit.Setup(building);
                buildingEdit.OnDelete += OnRemoveBuilding;
            }
            if (children == 3)
            {
                Add.Hide();
            }
        }
        
        private void NewBuildingEdit()
        {
            NewBuildingEdit(null);
        }
        
        private void OnRemoveBuilding(BuildingEdit Building)
        {
            Building.OnDelete -= OnRemoveBuilding;
            Add.Show();
        }
        
        private void ClearBuildingEdits()
        {
            foreach (Node node in Parent.GetChildren())
            {
                if (node is BuildingEdit buildingEdit)
                {
                    buildingEdit.QueueFree();
                }
            }
            Add.Show();
        }
	}
}
#endif
