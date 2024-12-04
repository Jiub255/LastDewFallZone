using System;

public class Timer
{
	public Action Action { get; set; }
	public float Duration { get; set; }
	
	public Timer(Action action, float duration)
	{
		Action = action;
		Duration = duration;
	}
	
	public void Tick(float delta)
	{
		Duration -= delta;
		if (Duration <= 0)
		{
			Action?.Invoke();
		}
	}
}

public class Timer<T>
{
	public Action<T> Action { get; set; }
	public T Argument { get; set; }
	public float Duration { get; set; }
	
	public Timer(Action<T> action, T argument, float duration)
	{
		Action = action;
		Argument = argument;
		Duration = duration;
	}
	
	public void Tick(float delta)
	{
		Duration -= delta;
		if (Duration <= 0)
		{
			Action?.Invoke(Argument);
		}
	}
}
