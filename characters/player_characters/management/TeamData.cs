using System;
using Godot;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Lastdew
{
	public partial class TeamData
	{
		public event Action OnMenuSelectedChanged;
		
		private readonly List<PlayerCharacter> _pcs = [];
		private int? _selectedIndex;
		private int _menuSelectedIndex;

		public ReadOnlyCollection<PlayerCharacter> Pcs => _pcs.AsReadOnly();

		// Whenever Selected gets set, set menu to that. But keep menu as it was when deselecting.
		// Changing Menu doesn't change Selected though. 
		public int? SelectedIndex
		{
			get => _selectedIndex;
			set
			{
				_selectedIndex = value;
				if (value != null)
				{
					MenuSelectedIndex = (int)value;
				}
			}
		}
		public int MenuSelectedIndex
		{
			get => _menuSelectedIndex;
			set
			{
				_menuSelectedIndex = value;
				OnMenuSelectedChanged?.Invoke();
			}
		}
		public List<PcSaveData> UnusedPcDatas { get; private set; } = [];
		public InventoryManager Inventory { get; } = new();
		

		public void SelectPc(PlayerCharacter pc)
		{
			int pcIndex = Pcs.IndexOf(pc);
			if (pcIndex == -1)
			{
				SelectedIndex = null;
				GD.PushWarning($"{pc.Name} not found in TeamData.Pcs.");
			}
			else
			{
				TurnOnPcSelectedIndicator(pc);
				SelectedIndex = pcIndex;
			}
		}
		
		public List<PcSaveData> GatherSaveData()
		{
			List<PcSaveData> pcSaveDatas = [];
			pcSaveDatas.AddRange(_pcs.Select(pc => new PcSaveData(pc)));
			return pcSaveDatas;
		}
		
		public void AddPc(PlayerCharacter pc)
		{
			_pcs.Add(pc);
		}
		
		public void ClearPcs()
		{
			_pcs.Clear();
		}

		public Dictionary<StatType, int> MaximumStats
		{
			get
			{
				Dictionary<StatType, int> maxStats = new()
				{
					{ StatType.ATTACK, 0 },
					{ StatType.DEFENSE, 0 },
					{ StatType.ENGINEERING, 0 },
					{ StatType.FARMING, 0 },
					{ StatType.MEDICAL, 0 },
					{ StatType.SCAVENGING, 0 }
				};
			
				foreach (PlayerCharacter pc in _pcs)
				{
					foreach (StatType statType in maxStats.Keys)
					{
						if (pc.StatManager.GetStatByType(statType).Value > maxStats[statType])
						{
							maxStats[statType] = pc.StatManager.GetStatByType(statType).Value;
						}
					}
				}
				return maxStats;
			}
		}

		private void TurnOnPcSelectedIndicator(PlayerCharacter pc)
		{
			ClearIndicators();
			pc.SetSelectedIndicator(true);
		}

		private void ClearIndicators()
		{
			foreach (PlayerCharacter pc in _pcs)
			{
				pc.SetSelectedIndicator(false);
			}
		}
	}
}
