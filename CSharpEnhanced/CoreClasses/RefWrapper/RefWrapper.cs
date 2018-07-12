namespace CSharpEnhanced.CoreClasses
{
	/// <summary>
	/// Class that may be used to wrap non-reference classes to be able to store them by reference
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RefWrapper<T>
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public RefWrapper() { }

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		public RefWrapper(T value)
		{
			Value = value;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Stored value
		/// </summary>
		public T Value { get; set; }

		#endregion
	}
}