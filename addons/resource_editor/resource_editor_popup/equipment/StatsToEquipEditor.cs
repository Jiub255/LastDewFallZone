#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class StatsToEquipEditor : StatAmountsEditor, IPropertyEditor
    {
        public void SetProperty(Craftable craftable)
        {
            if (craftable is Equipment equipment)
            {
                equipment.Set(Equipment.PropertyName.StatsNeededToEquip, Stats);
            }
        }
    }
}
#endif
