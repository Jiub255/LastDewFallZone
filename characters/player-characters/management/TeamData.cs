using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lastdew
{
	public partial class TeamData
	{
		public event Action OnPcsInstantiated;
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

		public TeamData() {}
		
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
				SelectedIndex = pcIndex;
			}
		}
		
		public List<PcSaveData> GatherSaveData()
		{
			List<PcSaveData> pcSaveDatas = [];
			foreach (PlayerCharacter pc in _pcs)
			{
				pcSaveDatas.Add(pc.GetSaveData());
			}
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
		
		public void SendPcsInstantiatedSignal()
		{
			OnPcsInstantiated?.Invoke();
		}
	}
}
