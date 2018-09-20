using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Class containing extension methods related to <see cref="IEnumerable{T}"/> and Linq.
	/// </summary>
	public static class LinqExtensions
	{
		#region Public static methods

		/// <summary>
		/// Concats the <paramref name="items"/> to the <paramref name="enumeration"/>
		/// </summary>
		/// <typeparam name="T">Type of enumeration</typeparam>
		/// <param name="enumeration">Enumeration to concat to</param>
		/// <param name="items">Items to concat</param>
		/// <returns></returns>
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumeration, params T[] items) =>
			// Check if enumeration isnt' null, cast items to IEnumerable<T> to use the linq method (otherwise recursion would occur)
			(enumeration ?? throw new ArgumentNullException(nameof(enumeration))).Concat(items as IEnumerable<T>);

		/// <summary>
		/// Performs a foreach loop on enumeration with the given action (enabling inline calls). Returns <paramref name="enumeration"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumeration"></param>
		/// <param name="action"></param>
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach(var item in enumeration)
			{
				action(item);
			}

			return enumeration;
		}

		#endregion
	}
}