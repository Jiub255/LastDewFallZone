#if TOOLS
using Godot;
using System;

namespace Lastdew
{
	[Tool]
	public partial class MaterialAmountEditor : PanelContainer
	{
        public event Action<MaterialAmountEditor> OnDelete;

        private long _craftingMaterial;
        
        public long CraftingMaterial
        {
            get => _craftingMaterial;
            private set
            {
                _craftingMaterial = value;
                SelectButton.Icon = Craftables?[value]?.Icon;
                SelectButton.Text = Craftables?[value]?.Name;
            }
        }
        public int Amount { get => (int)SpinBox.Value; }
        
        private SpinBox SpinBox { get; set; }
        private Button SelectButton { get; set; }
        private Button DeleteButton { get; set; }
        private EditorInterface EditorInterface { get; } = EditorInterface.Singleton;
        private Callable SetMaterialCallable { get; set; }
        private static Craftables Craftables => Databases.CRAFTABLES;

        public override void _Ready()
        {
            base._Ready();

            SpinBox = GetNode<SpinBox>("%SpinBox");
            SelectButton = GetNode<Button>("%Select");
            DeleteButton = GetNode<Button>("%Delete");

            SetMaterialCallable = new Callable(this, MethodName.SetMaterial);

            SelectButton.Pressed += ChooseMaterial;
            DeleteButton.Pressed += Delete;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            SelectButton.Pressed -= ChooseMaterial;
            DeleteButton.Pressed -= Delete;
        }
        
        public void Setup(long material, int amount)
        {
            CraftingMaterial = material;
            SpinBox.Value = amount;
        }
        
        private void ChooseMaterial()
        {
            EditorInterface.PopupQuickOpen(SetMaterialCallable, ["CraftingMaterial"]);
        }
        
        private void SetMaterial(string path)
        {
            CraftingMaterial = ResourceLoader.GetResourceUid(path);
        }
        
        private void Delete()
        {
            OnDelete?.Invoke(this);
            QueueFree();
        }
	}
}
#endif
