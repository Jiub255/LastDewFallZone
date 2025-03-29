#if TOOLS
using Godot;
using System;

namespace Lastdew
{
    [Tool]
    public partial class StatAmountUi : PanelContainer
    {
        public StatType Stat { get; private set; } = StatType.ATTACK;
        public int Amount { get => AmountLabel.Text.ToInt(); }
        
        private Label AmountLabel { get; set; }
        private Label StatLabel { get; set;}
        private Button Delete { get; set; }
        private StatAmountPopup StatAmountPopup { get; set; }

        public override void _Ready()
        {
            base._Ready();
            
            AmountLabel = GetNode<Label>("%Amount");
            StatLabel = GetNode<Label>("%Stat");
            Delete = GetNode<Button>("%Delete");
            StatAmountPopup = GetNode<StatAmountPopup>("%StatAmountPopup");

            Delete.Pressed += QueueFree;
            StatAmountPopup.OnClosePanel += Set;
        }

        public override void _ExitTree()
        {
            base._ExitTree();

            Delete.Pressed -= QueueFree;
            StatAmountPopup.OnClosePanel -= Set;
        }

        public override void _GuiInput(InputEvent @event)
        {
            base._GuiInput(@event);
            
            if (@event is InputEventMouseButton button && button.ButtonIndex == MouseButton.Left && button.Pressed)
            {
                OpenPopup();
            }
        }
        
        public void Setup(StatType statType, int amount, bool openPopup = false)
        {
            Stat = statType;
            StatLabel.Text = statType.ToString();
            AmountLabel.Text = amount.ToString();
            if (openPopup)
            {
                OpenPopup();
            }
        }
        
        private void Set(StatType statType, int amount)
        {
            Setup(statType, amount, false);
        }
        
        private void OpenPopup()
        {
            StatAmountPopup.Show();
            try
            {
                int amount = AmountLabel.Text.ToInt();
                StatAmountPopup.Setup(Stat, amount);
            }
            catch (Exception ex)
            {
                this.PrintDebug($"Amount label text not an int, setting to 1. {ex}");
                StatAmountPopup.Setup(Stat, 1);
            }
        }
    }
}
#endif
