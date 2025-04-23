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
        /* private ButtonGroup Group { get; set; } */
        private Button Attack { get; set; } 
        private Button Defense { get; set; } 
        private Button Engineering { get; set; } 
        private Button Farming { get; set; } 
        private Button Medical { get; set; } 
        private Button Scavenging { get; set; }
        private PanelContainer Panel { get; set; }

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
            Panel = GetNode<PanelContainer>("%PanelContainer");

            Attack.Pressed += () => AssignStat(Attack);
            Defense.Pressed += () => AssignStat(Defense);
            Engineering.Pressed += () => AssignStat(Attack);
            Farming.Pressed += () => AssignStat(Farming);
            Medical.Pressed += () => AssignStat(Medical);
            Scavenging.Pressed += () => AssignStat(Scavenging);

            /*  Attack.ButtonGroup = Group;
             Defense.ButtonGroup = Group;
             Engineering.ButtonGroup = Group;
             Farming.ButtonGroup = Group;
             Medical.ButtonGroup = Group;
             Scavenging.ButtonGroup = Group; */

            /* Group = Attack.ButtonGroup;

            Group.Pressed += AssignStat; */
            CloseRequested += Close;
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            
            Attack.Pressed -= () => AssignStat(Attack);
            Defense.Pressed -= () => AssignStat(Defense);
            Engineering.Pressed -= () => AssignStat(Attack);
            Farming.Pressed -= () => AssignStat(Farming);
            Medical.Pressed -= () => AssignStat(Medical);
            Scavenging.Pressed -= () => AssignStat(Scavenging);
            
           /*  Group.Pressed -= AssignStat; */
            CloseRequested -= Close;
        }

        /* public override void _GuiInput(InputEvent @event)
        {
            base._GuiInput(@event);
            
            if (@event.IsLeftClick())
            {
                Close();
            }
        } */
        
        public void Setup(StatType stat, int amount, Vector2 position)
        {
            Stat = stat;
            Amount.Value = amount;
            Panel.Position = position;
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
