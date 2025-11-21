#if TOOLS
using Godot;
using Godot.Collections;

namespace Lastdew
{
    [Tool]
    public partial class MaterialAmountsEditor : HBoxContainer
    {
        private VBoxContainer Parent { get; set; }
        private Button Add { get; set; }
        private PackedScene MaterialAmountScene { get; } = GD.Load<PackedScene>(Uids.MATERIAL_AMOUNT_EDITOR);
        protected Dictionary<long, int> Materials
        {
            get
            {
                return GatherMaterials();
            }
        }

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
        
        public void Setup(Dictionary<long, int> materialAmounts)
        {
            ClearMaterialAmounts();
            foreach (var kvp in materialAmounts)
            {
                NewMaterialAmount(kvp.Key, kvp.Value);
            }
        }
        
        private Dictionary<long, int> GatherMaterials()
        {
            Dictionary<long, int> materials;
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
