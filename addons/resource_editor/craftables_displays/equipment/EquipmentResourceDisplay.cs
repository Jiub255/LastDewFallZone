#if TOOLS
using System;
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class EquipmentResourceDisplay : CraftableDisplay
    {
        /* public event Action<Equipment> OnOpenPopupPressed;
        
        private Equipment Equipment { get; set; } */
        private Label Type { get; set; }
        private StatsDisplay EquipmentBonuses { get; set; }
        private StatsDisplay StatsNeededToEquip { get; set; }
    
        public override void _Ready()
        {
            base._Ready();

            Type = GetNode<Label>("%Type");
            EquipmentBonuses = GetNode<StatsDisplay>("%EquipmentBonuses");
            StatsNeededToEquip = GetNode<StatsDisplay>("%StatsNeededToEquip");
        }
    
        public override void Setup(Craftable craftable)
        {
            base.Setup(craftable);

            Equipment equipment = craftable as Equipment;
     //       Equipment = equipment;
            Type.Text = equipment.Type.ToString();
            EquipmentBonuses.Setup(equipment.EquipmentBonuses);
            StatsNeededToEquip.Setup(equipment.StatsNeededToEquip);
        }

    /*     protected override void OpenEditPopup()
        {
            OnOpenPopupPressed?.Invoke(Equipment);
        } */
    }
}
#endif
