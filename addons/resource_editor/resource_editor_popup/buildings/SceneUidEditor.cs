#if TOOLS
using Godot;

namespace Lastdew
{
	[Tool]
	public partial class SceneUidEditor : HBoxContainer, IPropertyEditor
	{
        private LineEdit LineEdit { get; set; }

        public override void _Ready()
        {
            base._Ready();

            LineEdit = GetNode<LineEdit>("%LineEdit");
        }
        
        public void Setup(string sceneUID)
        {
            LineEdit.Text = sceneUID;
        }

        public void SetProperty(Craftable craftable)
        {
            if (craftable is Building building)
            {
                building.Set(Building.PropertyName.SceneUid, LineEdit.Text);
            }
        }
    }
}
#endif
