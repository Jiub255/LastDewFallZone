#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class EquipmentBonusesEditor : StatAmountsEditor, IPropertyEditor
    {
        public void SetProperty(Craftable craftable)
        {
            if (craftable is Equipment equipment)
            {
                equipment.Set(Equipment.PropertyName.EquipmentBonuses, Stats);
            }
        }
    }
}
#endif
