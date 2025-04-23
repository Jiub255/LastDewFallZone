#if TOOLS
using Godot;
using System;
using System.Collections.Generic;

namespace Lastdew
{
    [Tool]
    public partial class StatAmountEdit : PanelContainer
    {
        public event Action<StatAmountEdit> OnDelete;
    
        public StatType Stat { get; private set; } = StatType.ATTACK;
        public SpinBox Amount { get; private set; }
        private Dictionary<long, StatType> StatsByIndex = new()
        {
            {0, StatType.ATTACK },
            {1, StatType.DEFENSE },
            {2, StatType.ENGINEERING },
            {3, StatType.FARMING },
            {4, StatType.MEDICAL },
            {5, StatType.SCAVENGING },
        };
        private MenuButton MenuButton { get; set; }
        private PopupMenu PopupMenu { get; set; }
        private Button DeleteButton { get; set; }

        public override void _Ready()
        {
            base._Ready();

            Amount = GetNode<SpinBox>("%SpinBox");
            MenuButton = GetNode<MenuButton>("%MenuButton");
            DeleteButton = GetNode<Button>("%Delete");
            
            PopupMenu = MenuButton.GetPopup();

            PopupMenu.IndexPressed += ChangeStat;
            DeleteButton.Pressed += Delete;
        }

        public override void _ExitTree()
        {
            base._ExitTree();

            PopupMenu.IndexPressed -= ChangeStat;
            DeleteButton.Pressed -= Delete;
        }
        
        public void Setup(StatType stat, int amount)
        {
            Stat = stat;
            MenuButton.Text = Stat.ToString().Capitalize();
            Amount.Value = amount;
        }

        private void ChangeStat(long index)
        {
            Stat = StatsByIndex[index];
            MenuButton.Text = Stat.ToString().Capitalize();
        }
        
        private void Delete()
        {
            OnDelete?.Invoke(this);
            QueueFree();
        }
    }
}
#endif
