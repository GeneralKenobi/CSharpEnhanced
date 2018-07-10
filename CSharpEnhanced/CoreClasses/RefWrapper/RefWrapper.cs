namespace CSharpEnhanced.CoreClasses
{
	/// <summary>
	/// Class that may be used to wrap non-reference classes to be able to store them by reference
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RefWrapper<T>
    {
		/// <summary>
		/// Stored value
		/// </summary>
		public T Value { get; set; }
    }
}
