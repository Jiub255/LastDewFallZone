#if TOOLS
using System;
using Godot;

namespace Lastdew
{
    [Tool]
    public abstract partial class CraftableDisplay : PanelContainer
    {
        public event Action<Craftable> OnOpenPopupPressed;

        private Craftable Craftable;
        private TextureRect Icon { get; set; }
        protected Label NameLabel { get; set; }
        private Label Description { get; set; }
        private StatsDisplay StatsNeededToCraft { get; set; }
        private RecipeCostsDisplay RecipeCosts { get; set; }
        private RequiredBuildingsDisplay RequiredBuildings { get; set; }
        private ScrapResultsDisplay ScrapResults { get; set; }

        public override void _Ready()
        {
            base._Ready();
            
            Icon = GetNode<TextureRect>("%Icon");
            NameLabel = GetNode<Label>("%Name");
            Description = GetNode<Label>("%Description");
            StatsNeededToCraft = GetNode<StatsDisplay>("%StatsNeededToCraft");
            RecipeCosts = GetNode<RecipeCostsDisplay>("%RecipeCosts");
            RequiredBuildings = GetNode<RequiredBuildingsDisplay>("%RequiredBuildings");
            ScrapResults = GetNode<ScrapResultsDisplay>("%ScrapResults");
            
            // TODO:
            //MouseEntered += // Set to hover color. Use selfmodulate to not conflict with button modulate?
            //MouseExited += // Set back to normal
        }

        public override void _GuiInput(InputEvent @event)
        {
            base._GuiInput(@event);
            
            if (@event is InputEventMouseButton button && button.ButtonIndex == MouseButton.Left)
            {
                if (button.Pressed)
                {
                    Modulate = new Color(0.65f, 0.65f, 0.65f, 1f);
                }
                else
                {
                    Modulate = new Color(1f, 1f, 1f, 1f);
                    OpenEditPopup();
                }
            }
        }
        
        public virtual void Setup(Craftable craftable)
        {
            Craftable = craftable;
            Icon.Texture = craftable.Icon;
            NameLabel.Text = craftable.Name;
            Description.Text = craftable.Description;
            Description.TooltipText = craftable.Description;
            StatsNeededToCraft.Setup(craftable.StatsNeededToCraft);
            RecipeCosts.Setup(craftable);
            RequiredBuildings.Setup(craftable);
            ScrapResults.Setup(craftable);
        }

        // TODO: Make abstract instead? Figure out later.
        private void OpenEditPopup()
        {
            // TODO: Setup Edit popup.
            OnOpenPopupPressed?.Invoke(Craftable);
        }
    }
}
#endif
