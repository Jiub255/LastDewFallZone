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
        private Dictionary<long, int> Materials
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
                this.PrintDebug($"Material: {material.Value} {Databases.CRAFTABLES[material.Key]?.Name}");
            }
            craftable.RecipeCosts = Materials;
        }
        
        private Dictionary<long, int> GatherMaterials()
        {
            Dictionary<long, int> materials;
            materials = [];
            foreach (Node node in Parent.GetChildren())
            {
                if (node is MaterialAmountEdit materialAmount)
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
            // TODO: Not sure this will work as a blank MaterialAmountEdit. It should?
            NewMaterialAmount(0, 1);
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
