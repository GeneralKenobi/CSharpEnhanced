namespace CSharpEnhanced.CoreInterfaces
{
	/// <summary>
	/// Interface for classes that implement shallow copying of objects
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IShallowCopyTo<T>
	{
		#region Methods

		/// <summary>
		/// Returns a shallow copy of object
		/// </summary>
		/// <returns></returns>
		T Copy();

		#endregion
	}
}