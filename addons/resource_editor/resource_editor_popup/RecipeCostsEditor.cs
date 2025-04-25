#if TOOLS
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class RecipeCostsEditor : MaterialAmountsEditor, IPropertyEditor
	{
        public void Save(Craftable craftable)
        {
            craftable.Set(Craftable.PropertyName.RecipeCosts, Materials);
        }
	}
}
#endif
