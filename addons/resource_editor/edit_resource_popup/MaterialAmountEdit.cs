#if TOOLS
using Godot;
using System;

namespace Lastdew
{
	[Tool]
	public partial class MaterialAmountEdit : PanelContainer
	{
        public event Action<MaterialAmountEdit> OnDelete;

        private CraftingMaterial _craftingMaterial;
        
        public CraftingMaterial CraftingMaterial
        {
            get => _craftingMaterial;
            private set
            {
                _craftingMaterial = value;
                SelectButton.Icon = value?.Icon;
                SelectButton.Text = value?.Name;
            }
        }
        public int Amount { get => (int)SpinBox.Value; }
        
        private SpinBox SpinBox { get; set; }
        private Button SelectButton { get; set; }
        private Button DeleteButton { get; set; }
        private EditorInterface EditorInterface { get; } = EditorInterface.Singleton;
        private Callable SetMaterialCallable { get; set; }

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
        
        public void Setup(CraftingMaterial material, int amount)
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
            CraftingMaterial = GD.Load<CraftingMaterial>(path);
        }
        
        private void Delete()
        {
            OnDelete?.Invoke(this);
            QueueFree();
        }
	}
}
#endif
