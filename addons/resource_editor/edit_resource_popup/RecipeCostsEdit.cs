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
    public partial class RecipeCostsEdit : HBoxContainer, IPropertyUi
    {
        private VBoxContainer Parent { get; set; }
        private Button Add { get; set; }
        private PackedScene MaterialAmountScene { get; } = GD.Load<PackedScene>(UIDs.MATERIAL_AMOUNT_EDIT);
        private Dictionary<CraftingMaterial, int> Materials
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
        
        public void Setup(Dictionary<CraftingMaterial, int> materialAmounts)
        {
            this.PrintDebug($"Setting up {materialAmounts.Count} material amounts");
            ClearMaterialAmounts();
            foreach (var kvp in materialAmounts)
            {
                NewMaterialAmount(kvp.Key, kvp.Value);
            }
        }
        
        public void Save(Craftable craftable)
        {
            this.PrintDebug($"Saving {craftable.Name}");
            foreach (var material in Materials)
            {
                this.PrintDebug($"Material: {material.Value} {material.Key.Name}");
            }
            craftable.Set(Craftable.PropertyName._recipeCosts, Materials);
        }
        
        private Dictionary<CraftingMaterial, int> GatherMaterials()
        {
            Dictionary<CraftingMaterial, int> materials;
            materials = [];
            foreach (Node node in Parent.GetChildren())
            {
                if (node is MaterialAmountEdit materialAmount && materialAmount.CraftingMaterial != null)
                {
                    materials[materialAmount.CraftingMaterial] = materialAmount.Amount;
                }
            }
            return materials;
        }
        
        private void NewMaterialAmount(CraftingMaterial material, int amount)
        {
            int children = Parent.GetChildren().Count;
            if (children <= 3)
            {
                MaterialAmountEdit materialAmount = (MaterialAmountEdit)MaterialAmountScene.Instantiate();
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
            NewMaterialAmount(null, 1);
        }
        
        private void OnRemoveMaterialAmount(MaterialAmountEdit materialAmount)
        {
            materialAmount.OnDelete -= OnRemoveMaterialAmount;
            Add.Show();
        }
        
        private void ClearMaterialAmounts()
        {
            foreach (Node node in Parent.GetChildren())
            {
                if (node is MaterialAmountEdit materialAmount)
                {
                    materialAmount.QueueFree();
                }
            }
            Add.Show();
        }
    }
}
#endif
