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
    public partial class StatsToCraftEdit : HBoxContainer, IPropertyUi
    {
        private VBoxContainer Parent { get; set; }
        private Button Add { get; set; }
        private PackedScene StatAmountScene { get; } = GD.Load<PackedScene>(UIDs.STAT_AMOUNT_EDIT);
        private Dictionary<StatType, int> Stats
        {
            get
            {
                return GatherStats();
            }
        }

        public override void _Ready()
        {
            base._Ready();
            
            Parent = GetNode<VBoxContainer>("%Parent");
            Add = GetNode<Button>("%Add");

            Add.Pressed += NewStatAmount;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            Add.Pressed -= NewStatAmount;
        }
        
        // TODO: Use Godot.Variant here instead of whatever collection in order to combine the classes?
        // Could just unpack it to whatever collection inside the method instead of before.
        public void Setup(Dictionary<StatType, int> statAmounts)
        {
            this.PrintDebug($"Setting up {statAmounts.Count} stat amounts");
            ClearStatAmounts();
            foreach (var kvp in statAmounts)
            {
                NewStatAmount(kvp.Key, kvp.Value);
            }
        }
        
        public void Save(Craftable craftable)
        {
            this.PrintDebug($"Saving {craftable.Name}");
            foreach (var stat in Stats)
            {
                this.PrintDebug($"Stat: {stat.Value} {stat.Key}");
            }
            craftable.Set(Craftable.PropertyName.StatsNeededToCraft, Stats);
        }
        
        // TODO: Use Variant here too? Or just put these inside the actual getter?
        private Dictionary<StatType, int> GatherStats()
        {
            Dictionary<StatType, int> stats;
            stats = [];
            foreach (Node node in Parent.GetChildren())
            {
                if (node is StatAmountEdit statAmount)
                {
                    stats[statAmount.Stat] = statAmount.Amount;
                }
            }
            return stats;
        }
        
        private void NewStatAmount(StatType stat, int amount)
        {
            int children = Parent.GetChildren().Count;
            if (children <= 3)
            {
                StatAmountEdit statAmount = (StatAmountEdit)StatAmountScene.Instantiate();
                Parent.AddChild(statAmount);
                statAmount.Setup(stat, amount);
                statAmount.OnDelete += OnRemoveStatAmount;
            }
            if (children == 3)
            {
                Add.Hide();
            }
        }
        
        private void NewStatAmount()
        {
            NewStatAmount(StatType.ATTACK, 1);
        }
        
        private void OnRemoveStatAmount(StatAmountEdit statAmount)
        {
            statAmount.OnDelete -= OnRemoveStatAmount;
            Add.Show();
        }
        
        private void ClearStatAmounts()
        {
            foreach (Node node in Parent.GetChildren())
            {
                if (node is StatAmountEdit statAmount)
                {
                    statAmount.QueueFree();
                }
            }
            Add.Show();
        }
    }
}
#endif
