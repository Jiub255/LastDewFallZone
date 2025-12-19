using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Lastdew
{
	public partial class EquipmentDisplay : VBoxContainer
	{
		public List<Button> Buttons => ButtonsByType.Values.ToList<Button>();
		private TeamData TeamData { get; set; }
		private Dictionary<EquipmentType, EquipmentButton> ButtonsByType { get; set; } = new();
		private PlayerCharacter Pc { get; set; }

		public override void _Ready()
		{
			base._Ready();

			ButtonsByType[EquipmentType.HEAD] = GetNode<EquipmentButton>("%HeadButton");
			ButtonsByType[EquipmentType.WEAPON] = GetNode<EquipmentButton>("%WeaponButton");
			ButtonsByType[EquipmentType.BODY] = GetNode<EquipmentButton>("%BodyButton");
			ButtonsByType[EquipmentType.FEET] = GetNode<EquipmentButton>("%FeetButton");

			foreach (KeyValuePair<EquipmentType, EquipmentButton> kvp in ButtonsByType)
			{
				EquipmentButton button = kvp.Value;
				button.Connect(EquipmentButton.SignalName.OnPressed, Callable.From<EquipmentType>(Unequip));
				button.Type = kvp.Key;
			}
		}

		public void ConnectSignals(TeamData teamData)
		{
			teamData.Connect(TeamData.SignalName.OnMenuSelectedChanged,
				Callable.From(SetNewPc));
		}

		public void Initialize(TeamData teamData)
		{
			TeamData = teamData;
		}

		public void Setup()
		{
			Pc = TeamData.Pcs[TeamData.MenuSelectedIndex];
			SetNewPc();
		}
		
		private void SetNewPc()
		{
			if (Pc != null)
			{
				Pc.OnEquipmentChanged -= SetupButtons;
			}
			Pc = TeamData.Pcs[TeamData.MenuSelectedIndex];
			Pc.OnEquipmentChanged += SetupButtons;
			SetupButtons();
		}
		
		private void SetupButtons()
		{
			PlayerCharacter menuCharacter = TeamData.Pcs[TeamData.MenuSelectedIndex];
			ButtonsByType[EquipmentType.HEAD].Equipment = menuCharacter.Equipment.Head;
			ButtonsByType[EquipmentType.WEAPON].Equipment = menuCharacter.Equipment.Weapon;
			ButtonsByType[EquipmentType.BODY].Equipment = menuCharacter.Equipment.Body;
			ButtonsByType[EquipmentType.FEET].Equipment = menuCharacter.Equipment.Feet;
		}
		
		private void Unequip(EquipmentType equipmentType)
		{
			TeamData.Pcs[TeamData.MenuSelectedIndex].Unequip(equipmentType);
			SetupButtons();
		}
	}
}
