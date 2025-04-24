#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class CraftableIcon : TextureRect
    {
        private Label Amount { get; set; }

        public override void _Ready()
        {
            base._Ready();
            
            Amount = GetNode<Label>("Amount");
        }

        public void Setup(Craftable craftable, string amount)
        {
            Texture = craftable?.Icon;
            TooltipText = craftable?.Name;
            Amount.Text = amount;
        }
    }
}
#endif
