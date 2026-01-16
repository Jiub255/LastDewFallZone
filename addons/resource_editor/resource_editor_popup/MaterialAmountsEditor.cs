#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class MaterialAmountsEditor : HBoxContainer
    {
        private VBoxContainer Parent { get; set; }
        private Button Add { get; set; }
        private PackedScene MaterialAmountScene { get; } = GD.Load<PackedScene>(Uids.MATERIAL_AMOUNT_EDITOR);
        protected Godot.Collections.Dictionary<long, int> Materials => GatherMaterials();

        public override void _Ready()
        {
            base._Ready();
            
            Parent = GetNode<VBoxContainer>("%Parent");
            Add = GetNode<Button>("%Add");

            Add.Pressed += NewMaterialAmount;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            Add.Pressed -= NewMaterialAmount;
        }
        
        public void Setup(Godot.Collections.Dictionary<long, int> materialAmounts)
        {
            ClearMaterialAmounts();
            if (materialAmounts == null)
            {
                return;
            }
            foreach ((long material, int amount) in materialAmounts)
            {
                NewMaterialAmount(material, amount);
            }
        }
        
        private Godot.Collections.Dictionary<long, int> GatherMaterials()
        {
            Godot.Collections.Dictionary<long, int> materials;
            materials = [];
            foreach (Node node in Parent.GetChildren())
            {
                if (node is MaterialAmountEditor materialAmount)
                {
                    materials[materialAmount.CraftingMaterial] = materialAmount.Amount;
                }
            }
            return materials;
        }
        
        private void NewMaterialAmount(long material, int amount)
        {
            int children = Parent.GetChildren().Count;
            if (children <= 3)
            {
                MaterialAmountEditor materialAmount = (MaterialAmountEditor)MaterialAmountScene.Instantiate();
                Parent.AddChild(materialAmount);
                materialAmount.Setup(material, amount);
                materialAmount.OnDelete += OnRemoveMaterialAmount;
            }
            if (children == 3)
            {
                Add.Hide();
            }
        }
        
        private void NewMaterialAmount()
        {
            NewMaterialAmount(0, 1);
        }
        
        private void OnRemoveMaterialAmount(MaterialAmountEditor materialAmount)
        {
            materialAmount.OnDelete -= OnRemoveMaterialAmount;
            Add.Show();
        }
        
        private void ClearMaterialAmounts()
        {
            foreach (Node node in Parent.GetChildren())
            {
                if (node is MaterialAmountEditor materialAmount)
                {
                    materialAmount.QueueFree();
                }
            }
            Add.Show();
        }
    }
}
#endif
