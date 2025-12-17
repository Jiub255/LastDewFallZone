#if TOOLS
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class BuildingTypeEditor : EnumEditor<BuildingType>, IPropertyEditor
	{
        public void SetProperty(Craftable craftable)
        {
            if (craftable is Building building)
            {
                building.Set(Building.PropertyName.BuildingType, (int)Enum);
            }
        }
    }
}
#endif
