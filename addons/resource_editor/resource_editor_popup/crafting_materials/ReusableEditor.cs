#if TOOLS
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class ReusableEditor : HBoxContainer, IPropertyEditor
	{
        private CheckBox CheckBox { get; set; }

        public override void _Ready()
        {
            base._Ready();

            CheckBox = GetNode<CheckBox>("%CheckBox");
        }

        public void Setup(bool reusable)
        {
            CheckBox.ButtonPressed = reusable;
        }

        public void SetProperty(Craftable craftable)
        {
            if (craftable is CraftingMaterial craftingMaterial)
            {
                craftingMaterial.Set(CraftingMaterial.PropertyName.Reusable, CheckBox.ButtonPressed);
            }
            else if (craftable is UsableItem usableItem)
            {
                usableItem.Set(UsableItem.PropertyName.Reusable, CheckBox.ButtonPressed);
            }
        }
	}
}
#endif
