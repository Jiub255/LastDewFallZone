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

        private StatType _stat;
    
        public StatType Stat
        {
            get => _stat;
            private set
            {
                _stat = value;
                if (MenuButton != null)
                {
                    MenuButton.Text = value.ToString().Capitalize();
                }
            }
        }
        public int Amount { get => (int)SpinBox.Value; }
        
        private readonly Dictionary<long, StatType> StatsByIndex = new()
        {
            {0, StatType.ATTACK },
            {1, StatType.DEFENSE },
            {2, StatType.ENGINEERING },
            {3, StatType.FARMING },
            {4, StatType.MEDICAL },
            {5, StatType.SCAVENGING },
        };
        private SpinBox SpinBox { get; set; }
        private MenuButton MenuButton { get; set; }
        private PopupMenu PopupMenu { get; set; }
        private Button DeleteButton { get; set; }

        public override void _Ready()
        {
            base._Ready();

            SpinBox = GetNode<SpinBox>("%SpinBox");
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
            SpinBox.Value = amount;
        }

        private void ChangeStat(long index)
        {
            Stat = StatsByIndex[index];
        }
        
        private void Delete()
        {
            OnDelete?.Invoke(this);
            QueueFree();
        }
    }
}
#endif
