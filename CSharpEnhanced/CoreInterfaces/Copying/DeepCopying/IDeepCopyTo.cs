namespace CSharpEnhanced.CoreInterfaces
{
	/// <summary>
	/// Interface for classes that implement deep copying of objects
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IDeepCopyTo<T>
    {
		#region Methods

		/// <summary>
		/// Returns a deep copy of object
		/// </summary>
		/// <returns></returns>
		T Copy();

		#endregion
	}
}