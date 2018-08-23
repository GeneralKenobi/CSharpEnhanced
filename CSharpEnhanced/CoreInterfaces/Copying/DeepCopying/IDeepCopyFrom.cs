namespace CSharpEnhanced.CoreInterfaces
{
	/// <summary>
	/// Interface for classes that allow for deep copying into self
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IDeepCopyFrom<T>
	{
		#region Methods

		/// <summary>
		/// Copies contents of <paramref name="obj"/> to this object		
		/// </summary>
		/// <returns></returns>
		void CopyFrom(T obj);

		#endregion
	}
}