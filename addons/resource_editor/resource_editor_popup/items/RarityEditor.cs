#if TOOLS
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class RarityEditor : EnumEditor<Rarity>, IPropertyEditor
	{
        public void SetProperty(Craftable craftable)
        {
            if (craftable is Item item)
            {
                item.Set(Item.PropertyName.ItemRarity, (int)Enum);
            }
        }
    }
}
#endif
