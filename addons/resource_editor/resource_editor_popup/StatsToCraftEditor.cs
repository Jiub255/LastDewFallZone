#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class StatsToCraftEditor : StatAmountsEditor, IPropertyEditor
    {
        public void SetProperty(Craftable craftable)
        {
            craftable.Set(Craftable.PropertyName.StatsNeededToCraft, Stats);
        }
    }
}
#endif
