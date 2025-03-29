#if TOOLS
using Godot;
using System;
using System.Collections.Generic;

namespace Lastdew
{
    [Tool]
    public partial class StatsToCraftUi : HBoxContainer, IPropertyUi
    {
        private const string STATS_TO_CRAFT = "StatsNeededToCraft";
    
        private HBoxContainer Parent { get; set; }
        private Button Add { get; set; }
        private PackedScene StatAmountScene { get; } = GD.Load<PackedScene>(UIDs.STAT_AMOUNT_UI);
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
            
            Parent = GetNode<HBoxContainer>("%Parent");
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
                if (node is StatAmountUi statAmount)
                {
                    stats[statAmount.Stat] = statAmount.Amount;
                }
            }
            return stats;
        }
        
        private void SetupStatAmount(StatType stat, int amount, bool openPopup = false)
        {
            StatAmountUi statAmount = (StatAmountUi)StatAmountScene.Instantiate();
            Parent.AddChild(statAmount);
            statAmount.Setup(stat, amount, openPopup);
        }
        
        private void NewStatAmount()
        {
            int children = Parent.GetChildren().Count;
            if (children <= 3)
            {
                SetupStatAmount(StatType.ATTACK, 1, true);
            }
            if (children == 3)
            {
                Add.Hide();
            }
        }
        
        private void OnRemoveStatAmount()
        {
            Add.Show();
        }
        
        public void Save(Craftable craftable)
        {
            craftable.Set(STATS_TO_CRAFT, Stats);
        }
    }
}
#endif
