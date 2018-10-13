using System.Collections;

namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Contains helper methods related to <see cref="IEnumerator{T}"/> and <see cref="IEnumerator"/>
	/// </summary>
	public static class EnumeratorHelpers
	{
		#region Public methods

		/// <summary>
		/// Advances the iterator n elements forward, returns true on success.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerator"></param>
		/// <param name="n">Number of positions to advance forward</param>
		/// <returns></returns>
		public static bool MoveNext(this IEnumerator enumerator, int n)
		{
			for(int i=0; i<n; ++i)
			{
				if(!enumerator.MoveNext())
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Moves to the next element. If the action exceeded the enumeration, returns to the first element. Returns true on success.
		/// Returns false on failure - when enumeration is empty and it's not possible to reach any element.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerator"></param>
		/// <returns></returns>
		public static bool MoveNextWrapping(this IEnumerator enumerator)
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

		/// <summary>
		/// Moves n positions forward. If the action exceeded the enumeration, returns to the first element. Returns true on success.
		/// Returns false on failure - when enumeration is empty and it's not possible to reach any element.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerator"></param>
		/// <param name="n">Number of positions to advance forward</param>
		/// <returns></returns>
		public static bool MoveNextWrapping(this IEnumerator enumerator, int n)
		{
			for (int i = 0; i < n; ++i)
			{
				if (!enumerator.MoveNextWrapping())
				{
					return false;
				}
			}

			return true;
		}

		#endregion
	}
}