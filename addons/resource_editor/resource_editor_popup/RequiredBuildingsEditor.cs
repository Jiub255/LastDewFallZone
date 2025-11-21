#if TOOLS
using Godot;
using Godot.Collections;

namespace Lastdew
{
	[Tool]
	public partial class RequiredBuildingsEditor : HBoxContainer, IPropertyEditor
	{
        private HBoxContainer Parent { get; set; }
        private Button Add { get; set; }
        private PackedScene BuildingEditScene { get; } = GD.Load<PackedScene>(Uids.BUILDING_EDITOR);
        private Array<long> Buildings
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
        
        public void Setup(Array<long> buildings)
        {
            ClearBuildingEdits();
            foreach (long building in buildings)
            {
                NewBuildingEdit(building);
            }
        }
        
        public void SetProperty(Craftable craftable)
        {
            craftable.Set(Craftable.PropertyName.RequiredBuildings, Buildings);
        }
        
        private Array<long> GatherBuildings()
        {
            Array<long> buildings;
            buildings = [];
            foreach (Node node in Parent.GetChildren())
            {
                if (node is BuildingEditor buildingEdit)
                {
                    buildings.Add(buildingEdit.Building);
                }
            }
            return buildings;
        }
        
        private void NewBuildingEdit(long building)
        {
            int children = Parent.GetChildren().Count;
            if (children <= 3)
            {
                BuildingEditor buildingEdit = (BuildingEditor)BuildingEditScene.Instantiate();
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
            NewBuildingEdit(0);
        }
        
        private void OnRemoveBuilding(BuildingEditor building)
        {
            building.OnDelete -= OnRemoveBuilding;
            Add.Show();
        }
        
        private void ClearBuildingEdits()
        {
            foreach (Node node in Parent.GetChildren())
            {
                if (node is BuildingEditor buildingEdit)
                {
                    buildingEdit.QueueFree();
                }
            }
            Add.Show();
        }
	}
}
#endif
