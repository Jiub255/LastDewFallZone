using System;
using System.Collections.Generic;

namespace Lastdew
{	
	public class PcHealth
	{
		public event Action OnPainChanged;
		
		private const int MAX_INJURY = 100;
	
		private int _injury;
		
		public int Injury
		{
			get => _injury;
			private set
			{
				_injury = value;
				OnPainChanged?.Invoke();
			}
		}
		
		// TODO: Have high pain affect your stats negatively.
		// Do it in a way that makes sense.
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
		
		private class Relief
		{
			public int Amount { get; set; }
			public float Duration { get; set; }
			
			public Relief(int amount, float duration)
			{
				Amount = amount;
				Duration = duration;
			}
		}
		
		private List<Relief> Reliefs { get; set; } = new List<Relief>();
		
		public PcHealth() {}
		
		public void ProcessRelief(float delta)
		{
			foreach (Relief relief in Reliefs)
			{
				relief.Duration -= delta;
				if (relief.Duration < 0)
				{
					Reliefs.Remove(relief);
					OnPainChanged?.Invoke();
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
			OnPainChanged?.Invoke();
		}
	}
}
