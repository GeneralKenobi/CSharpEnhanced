using System.Collections.Generic;

namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Contains helper methods related to <see cref="IEnumerator{T}"/>
	/// </summary>
	public static class EnumeratorHelpers
	{
		#region Public methods

		/// <summary>
		/// Moves to the next element. If the action exceeded the enumeration, returns to the first element. Returns true on success.
		/// Returns false on failure - when enumeration is empty and it's not possible to reach any element.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerator"></param>
		/// <returns></returns>
		public static bool MoveNextWrapping<T>(this IEnumerator<T> enumerator)
		{
			// Try to move to the next element
			if(enumerator.MoveNext())
			{
				// On success return success (enumeration was not exceeded)
				return true;
			}
			else
			{
				// On failure return to the beginning
				enumerator.Reset();

				// And move to the first element. Return the result of the operation (for empty sequences it's not possible to
				// reach the first element and it's not possible to wrap around the end - it needs to be signaled)
				return enumerator.MoveNext();
			}
		}

		#endregion
	}
}