namespace TypedRoute
{
	public class ItIs
	{
		public static T Any<T>()
		{
			return default(T);
		}
	}
}