#if TOOLS
using System;
using Godot;
    
namespace Lastdew    
{
    [Tool]
    public partial class BuildingsDisplay : CraftableDisplay
    {
        /* public event Action<Building> OnOpenPopupPressed;
    
        private Building Building { get; set; } */
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
          //  Building = building;
            Type.Text = building.Type.ToString();
            Scene.Text = building.Scene != null ? building.Scene.ResourcePath : "";
        }

        /* protected override void OpenEditPopup()
        {
            OnOpenPopupPressed?.Invoke(Building);
        } */
    }
}
#endif
