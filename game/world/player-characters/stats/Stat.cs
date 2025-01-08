using System;
using System.Collections.Generic;

namespace Lastdew
{
	public enum StatType
	{
		ATTACK,
		DEFENSE,
		ENGINEERING,
		FARMING,
		MEDICAL,
		SCAVENGING,
	}
	
	public class Stat
	{	
		public event Action OnBaseValueChanged;
		
		private int _baseValue = 1;
		
		public int BaseValue
		{
			get => _baseValue;
			set
			{
				_baseValue = value;
				CalculateValue();
				OnBaseValueChanged?.Invoke();
			}
		}
		public StatType Type { get; set; }
		public int Value { get; private set; }
	
		private List<int> Modifiers { get; } = new();
		
		public Stat(StatType type, int baseValue)
		{
			Type = type;
			BaseValue = baseValue;
		}
		
		public void AddModifier(int modifier)
		{
			Modifiers.Add(modifier);
			CalculateValue();
		}
		
		public void RemoveModifier(int modifier)
		{
			Modifiers.Remove(modifier);
			CalculateValue();
		}
		
		public void ClearModifiers()
		{
			Modifiers.Clear();
			CalculateValue();
		}
		
		private void CalculateValue()
		{
			Value = BaseValue;
			foreach (int mod in Modifiers)
			{
				Value += mod;
			}
		}
	}
}
