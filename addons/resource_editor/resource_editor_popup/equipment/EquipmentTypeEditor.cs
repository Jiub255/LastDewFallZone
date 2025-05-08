#if TOOLS
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class EquipmentTypeEditor : EnumEditor<EquipmentType>, IPropertyEditor
	{
        public void SetProperty(Craftable craftable)
        {
            if (craftable is Equipment equipment)
            {
                equipment.Set(Equipment.PropertyName.Type, (int)Enum);
            }
        }
    }
}
#endif
