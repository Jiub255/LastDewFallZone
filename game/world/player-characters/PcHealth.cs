using System.Collections.Generic;

public class PcHealth
{
	private int Injury { get; set; }
	
	public int Pain
	{
		get
		{
			int pain = Injury;
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
	private List<Relief> Reliefs { get; set; }
	
	public PcHealth() {}
	
	public void ProcessRelief(float delta)
	{
		foreach (Relief relief in Reliefs)
		{
			relief.Duration -= delta;
			if (relief.Duration < 0)
			{
				Reliefs.Remove(relief);
			}
		}
	}
	
	public void TakeDamage(int damage)
	{
		Injury += damage;
		if (Injury > 100)
		{
			Injury = 100;
			// TODO: Become incapacitated until healing at home base. Or die?
		}
	}
	
	public void RelievePain(int amount, float duration)
	{
		Reliefs.Add(new Relief(amount, duration));
	}
}
