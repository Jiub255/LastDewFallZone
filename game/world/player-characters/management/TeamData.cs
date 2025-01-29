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

		// ------------------------ DATA TO SAVE ------------------------

		public List<int> PcIndexes { get; } = new List<int>();

		// --------------------------------------------------------------

		public ReadOnlyCollection<PlayerCharacter> Pcs
		{
			get => _pcs.AsReadOnly();
		}
		public List<PackedScene> PcScenes
		{
			get
			{
				List<PackedScene> pcScenes = new();
				foreach (int index in PcIndexes)
				{
					pcScenes.Add(AllPcs.PcScenes[index]);
				}
				return pcScenes;
			}
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
		
		public TeamData(AllPcScenes allPcs, List<int> pcIndexes)
		{
			AllPcs = allPcs;
			PcIndexes = pcIndexes;
		}
		
		public void SpawnPcs(PcManagerBase pcManager, InventoryManager inventoryManager)
		{
			for (int i = 0; i < PcScenes.Count; i++)
			{
				PlayerCharacter pc = (PlayerCharacter)PcScenes[i].Instantiate();
				pcManager.CallDeferred(PcManagerBase.MethodName.AddChild, pc);
				pc.Position += Vector3.Right * i * 3;
				pc.Initialize(inventoryManager);
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
	}
}
