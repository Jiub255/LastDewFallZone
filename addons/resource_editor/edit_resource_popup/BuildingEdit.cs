#if TOOLS
using Godot;
using System;

namespace Lastdew
{
	[Tool]
	public partial class BuildingEdit : PanelContainer
	{
        public event Action<BuildingEdit> OnDelete;

        private long _building;
        
        public long Building
        {
            get => _building;
            private set
            {
                _building = value;
                BuildingButton.Icon = Craftables?[value]?.Icon;
            }
        }
        
        private Button BuildingButton { get; set; }
        private Button DeleteButton { get; set; }
        private EditorInterface EditorInterface { get; } = EditorInterface.Singleton;
        private Callable SetBuildingCallable { get; set; }
        private Craftables Craftables { get; } = Databases.CRAFTABLES;

        public override void _Ready()
        {
            base._Ready();

            BuildingButton = GetNode<Button>("%ChooseBuilding");
            DeleteButton = GetNode<Button>("%Delete");

            SetBuildingCallable = new Callable(this, MethodName.SetBuilding);

            BuildingButton.Pressed += ChooseBuilding;
            DeleteButton.Pressed += Delete;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            BuildingButton.Pressed -= ChooseBuilding;
            DeleteButton.Pressed -= Delete;
        }
        
        public void Setup(long building)
        {
            Building = building;
        }
        
        private void ChooseBuilding()
        {
            EditorInterface.PopupQuickOpen(SetBuildingCallable, ["Building"]);
        }
        
        private void SetBuilding(string path)
        {
            Building = ResourceLoader.GetResourceUid(path);
        }
        
        private void Delete()
        {
            OnDelete?.Invoke(this);
            QueueFree();
        }
	}
}
#endif
