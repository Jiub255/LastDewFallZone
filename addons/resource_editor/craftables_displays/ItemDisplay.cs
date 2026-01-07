#if TOOLS
using System.Collections.Generic;
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class ItemDisplay : CraftableDisplay
	{
        private Label Rarity { get; set; }
        private Label Tags { get; set; }

        public override void _Ready()
        {
            base._Ready();
            
            Rarity = GetNode<Label>("%Rarity");
            Tags = GetNode<Label>("%Tags");
        }

        public override void Setup(Craftable craftable)
        {
            base.Setup(craftable);
            
            Item item = craftable as Item;
            Rarity.Text = item.Rarity.ToString();
            Rarity.TooltipText = item.Rarity.ToString();
            List<string> tags = [];
            foreach (ItemTags tag in item.Tags)
            {
                tags.Add(tag.ToString());
            }
            Tags.Text = tags.ToArray().Join(" ");
            Tags.TooltipText = tags.ToArray().Join(" ");
        }
	}
}
#endif
