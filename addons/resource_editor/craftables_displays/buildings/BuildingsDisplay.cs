#if TOOLS
using Godot;
    
namespace Lastdew    
{
    [Tool]
    public partial class BuildingsDisplay : CraftableDisplay
    {
        private Label Type { get; set; }
        private Label Scene { get; set; }

        public override void _Ready()
        {
            base._Ready();
            
            Type = GetNode<Label>("%Type");
            Scene = GetNode<Label>("%Scene");
        }

        public override void Setup(Craftable craftable)
        {
            base.Setup(craftable);
            
            Building building = craftable as Building;
            Type.Text = building.Type.ToString();
            Scene.Text = building.SceneUid;
            Scene.TooltipText = building.SceneUid;
        }
    }
}
#endif
