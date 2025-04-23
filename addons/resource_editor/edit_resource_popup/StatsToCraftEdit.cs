#if TOOLS
using Godot;
using System.Collections.Generic;

namespace Lastdew
{
    [Tool]
    public partial class StatsToCraftEdit : HBoxContainer, IPropertyUi
    {
        private VBoxContainer Parent { get; set; }
        private Button Add { get; set; }
        private PackedScene StatAmountScene { get; } = GD.Load<PackedScene>(UIDs.STAT_AMOUNT_EDIT);
        private Godot.Collections.Dictionary<StatType, int> Stats
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
        
        public void Setup(Godot.Collections.Dictionary<StatType, int> statAmounts)
        {
            this.PrintDebug($"Setting up {statAmounts.Count} stat amounts");
            foreach (KeyValuePair<StatType, int> kvp in statAmounts)
            {
                SetupStatAmount(kvp.Key, kvp.Value);
            }
        }
        
        private Godot.Collections.Dictionary<StatType, int> GatherStats()
        {
            Godot.Collections.Dictionary<StatType, int> stats = [];
            foreach (Node node in Parent.GetChildren())
            {
                if (node is StatAmountEdit statAmount)
                {
                    stats[statAmount.Stat] = (int)statAmount.Amount.Value;
                }
            }
            return stats;
        }
        
        private void SetupStatAmount(StatType stat, int amount)
        {
            StatAmountEdit statAmount = (StatAmountEdit)StatAmountScene.Instantiate();
            Parent.AddChild(statAmount);
            statAmount.Setup(stat, amount);
            statAmount.OnDelete += OnRemoveStatAmount;
        }
        
        private void NewStatAmount()
        {
            int children = Parent.GetChildren().Count;
            if (children <= 3)
            {
                SetupStatAmount(StatType.ATTACK, 1);
            }
            if (children == 3)
            {
                Add.Hide();
            }
        }
        
        private void OnRemoveStatAmount(StatAmountEdit statAmount)
        {
            statAmount.OnDelete -= OnRemoveStatAmount;
            Add.Show();
        }
        
        public void Save(Craftable craftable)
        {
            this.PrintDebug($"Saving {craftable.Name}");
            foreach (KeyValuePair<StatType, int> stat in Stats)
            {
                this.PrintDebug($"Stat: {stat.Value} {stat.Key}");
            }
            craftable.Set(Craftable.PropertyName.StatsNeededToCraft, Stats);
        }
    }
}
#endif
