public class Pair<T, S>
{
	public T Item1 { get; set; }
	public S Item2 { get; set; }
	
	public Pair(T item1, S item2)
	{
		Item1 = item1;
		Item2 = item2;
	}
}
