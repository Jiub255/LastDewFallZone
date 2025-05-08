#if TOOLS
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class ScrapResultsEditor : MaterialAmountsEditor, IPropertyEditor
    {
        public void SetProperty(Craftable craftable)
        {
            craftable.Set(Craftable.PropertyName.ScrapResults, Materials);
        }
    }
}
#endif
