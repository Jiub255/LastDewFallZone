using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lastdew
{
	/// <summary>
	/// TODO: Only inheriting RefCounted to get passed in a CallDeferred in Hud class.
	/// Hopefully find a better way eventually. 
	/// </summary>
	public partial class TeamData : RefCounted
	{
		public event Action OnPcsInstantiated;
		public event Action OnMenuSelectedChanged;
		
		private List<PlayerCharacter> _pcs = new();
		private int? _selectedIndex;
		private int _menuSelectedIndex;

		public ReadOnlyCollection<PlayerCharacter> Pcs
		{
			get => _pcs.AsReadOnly();
		}
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

		private AllPcScenes AllPcs { get; }
		
		public TeamData()
		{
			AllPcs = GD.Load<AllPcScenes>("res://game/world/player-characters/management/all_pc_scenes.tres");
		}
		
		/// <summary>
		/// TODO: Figure out how to pass down the loaded pc data. This is getting ridiculous passing this list around.
		/// </summary>
		public void SpawnPcs(PcManagerBase pcManager, InventoryManager inventoryManager, List<PcSaveData> pcSaveDatas)
		{
			int i = 0;
			foreach (PcSaveData pcSaveData in pcSaveDatas)
			{
				PlayerCharacter pc = (PlayerCharacter)AllPcs[pcSaveData.Name].Instantiate();
				pcManager.CallDeferred(PcManagerBase.MethodName.AddChild, pc);
				// TODO: Add a spawn location for pcs.
				pc.Position += Vector3.Right * i * 3;
				i++;
				pc.Initialize(inventoryManager, pcSaveData);
				_pcs.Add(pc);
			}
			
			OnPcsInstantiated?.Invoke();
		}
		
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
		
		public List<PcSaveData> GetSaveData()
		{
			List<PcSaveData> pcSaveDatas = new();
			foreach (PlayerCharacter pc in _pcs)
			{
				pcSaveDatas.Add(pc.GetSaveData());
			}
			return pcSaveDatas;
		}
	}
}
