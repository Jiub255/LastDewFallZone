#if TOOLS
using Godot;
using System;

namespace Lastdew
{
    [Tool]
    public partial class StatAmountEditor : PanelContainer
    {
        public event Action<StatAmountEditor> OnDelete;

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
            PopupMenu.Clear();
            foreach (StatType statType in Enum.GetValues<StatType>())
            {
                PopupMenu.AddItem(statType.ToString().Capitalize());
            }

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
            Stat = (StatType)index;
        }
        
        private void Delete()
        {
            OnDelete?.Invoke(this);
            QueueFree();
        }
    }
}
#endif
