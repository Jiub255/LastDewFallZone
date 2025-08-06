using System;
using System.Collections.Generic;

namespace Lastdew
{	
	public class PcHealth
	{
		/// <summary>
		/// Sends Pain int
		/// </summary>
		public event Action<int> OnHealthChanged;
		
		private const int MAX_INJURY = 100;
	
		private int _injury;
		
		public int Injury
		{
			get => _injury;
			private set
			{
				_injury = value;
				OnHealthChanged?.Invoke(Pain);
			}
		}
		
		// TODO: Have high pain affect your stats negatively.
		// Do it in a way that makes sense.
		// TODO: Lower amount of relief over time? Or have it follow some curve down?
		// OR just have it last until you get back home?
		public int Pain
		{
			get
			{
				int pain = _injury;
				foreach (Relief relief in Reliefs)
				{
					pain -= relief.Amount;
				}
				if (pain < 0)
				{
					pain = 0;
				}
				return pain;
			}
		}
		
		private class Relief(int amount, float duration)
        {
            public int Amount { get; } = amount;
            public float Duration { get; set; } = duration;
        }
		
		private List<Relief> Reliefs { get; set; } = [];
		
		public PcHealth(PcSaveData pcSaveData)
		{
			Injury = pcSaveData.Injury;
		}
		
		public void ProcessRelief(float delta)
		{
			for (int i = Reliefs.Count - 1; i >= 0; i--)
			{
				Reliefs[i].Duration -= delta;
				if (Reliefs[i].Duration < 0)
				{
					Reliefs.RemoveAt(i);
					OnHealthChanged?.Invoke(Pain);
				}
			}
		}
		
		/// <returns>true if Injury at max.</returns>
		public bool TakeDamage(int damage)
		{
			Injury += damage;
			if (Injury > MAX_INJURY)
			{
				Injury = MAX_INJURY;
				// TODO: Become incapacitated until healing at home base. Or die?
				return true;
			}
			return false;
		}
		
		public void RelievePain(int amount, float duration)
		{
			Reliefs.Add(new Relief(amount, duration));
			OnHealthChanged?.Invoke(Pain);
			this.PrintDebug($"Injury: {Injury}, Pain: {Pain}");
		}
	}
}
