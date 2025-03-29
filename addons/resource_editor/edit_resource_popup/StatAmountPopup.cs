#if TOOLS
using Godot;
using System;

namespace Lastdew
{
    [Tool]
    public partial class StatAmountPopup : PopupPanel
    {
        public event Action<StatType, int> OnClosePanel;
    
        private StatType Stat { get; set; }
    
        private SpinBox Amount { get; set; }
        private ButtonGroup Group { get; set; } = new ButtonGroup();
        private Button Attack { get; set; } 
        private Button Defense { get; set; } 
        private Button Engineering { get; set; } 
        private Button Farming { get; set; } 
        private Button Medical { get; set; } 
        private Button Scavenging { get; set; }

        public override void _Ready()
        {
            base._Ready();

            Amount = GetNode<SpinBox>("%SpinBox");

            Attack = GetNode<Button>("%Attack");
            Defense = GetNode<Button>("%Defense");
            Engineering = GetNode<Button>("%Engineering");
            Farming = GetNode<Button>("%Farming");
            Medical = GetNode<Button>("%Medical");
            Scavenging = GetNode<Button>("%Scavenging");

            Attack.ButtonGroup = Group;
            Defense.ButtonGroup = Group;
            Engineering.ButtonGroup = Group;
            Farming.ButtonGroup = Group;
            Medical.ButtonGroup = Group;
            Scavenging.ButtonGroup = Group;

            Group.Pressed += AssignStat;
            CloseRequested += Close;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            Group.Pressed -= AssignStat;
            CloseRequested -= Close;
        }
        
        public void Setup(StatType stat, int amount)
        {
            Stat = stat;
            Amount.Value = amount;
        }

        private void AssignStat(BaseButton button)
        {
            Stat = (string)button.Name switch
            {
                "Attack" => StatType.ATTACK,
                "Defense" => StatType.DEFENSE,
                "Engineering" => StatType.ENGINEERING,
                "Farming" => StatType.FARMING,
                "Medical" => StatType.MEDICAL,
                "Scavenging" => StatType.SCAVENGING,
                _ => StatType.ATTACK,
            };
        }

        private void Close()
        {
            OnClosePanel?.Invoke(Stat, (int)Amount.Value);
            Hide();
        }
    }
}
#endif
