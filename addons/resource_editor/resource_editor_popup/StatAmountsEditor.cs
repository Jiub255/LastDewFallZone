#if TOOLS
using Godot;
using Godot.Collections;

namespace Lastdew
{
    [Tool]
    public partial class StatAmountsEditor : HBoxContainer
    {
        private VBoxContainer Parent { get; set; }
        private Button Add { get; set; }
        private PackedScene StatAmountScene { get; } = GD.Load<PackedScene>(UiDs.STAT_AMOUNT_EDITOR);
        protected Dictionary<StatType, int> Stats
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

        public void Setup(Dictionary<StatType, int> statAmounts)
        {
            ClearStatAmounts();
            foreach (var kvp in statAmounts)
            {
                NewStatAmount(kvp.Key, kvp.Value);
            }
        }
        
        private Dictionary<StatType, int> GatherStats()
        {
            Dictionary<StatType, int> stats;
            stats = [];
            foreach (Node node in Parent.GetChildren())
            {
                if (node is StatAmountEditor statAmount)
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
                StatAmountEditor statAmount = (StatAmountEditor)StatAmountScene.Instantiate();
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
        
        private void OnRemoveStatAmount(StatAmountEditor statAmount)
        {
            statAmount.OnDelete -= OnRemoveStatAmount;
            Add.Show();
        }
        
        private void ClearStatAmounts()
        {
            foreach (Node node in Parent.GetChildren())
            {
                if (node is StatAmountEditor statAmount)
                {
                    statAmount.QueueFree();
                }
            }
            Add.Show();
        }
    }
}
#endif
