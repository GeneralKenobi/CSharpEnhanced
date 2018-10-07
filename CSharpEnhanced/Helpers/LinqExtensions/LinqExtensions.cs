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
		/// Concats <paramref name="items"/> with <paramref name="enumeration"/>
		/// </summary>
		/// <typeparam name="T">Type of enumeration</typeparam>
		/// <param name="enumeration">Enumeration to concat to</param>
		/// <param name="items">Items to concat</param>
		/// <returns></returns>
		public static IEnumerable<T> ConcatAtBeginning<T>(this IEnumerable<T> enumeration, params T[] items) =>
			// Check if enumeration isnt' null, cast items to IEnumerable<T> to use the linq method (otherwise recursion would occur)
			items.Concat(enumeration ?? throw new ArgumentNullException(nameof(enumeration)));

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

		/// <summary>
		/// Performs a foreach loop on enumeration with the given action (enabling inline calls). Returns <paramref name="enumeration"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumeration"></param>
		/// <param name="action"></param>
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
		{
			int i = 0;

			foreach (var item in enumeration)
			{
				action(item, i);

				++i;
			}

			return enumeration;
		}

		/// <summary>
		/// Filters a sequence to elements for which all of <paramref name="predicates"/> return true.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumeration"></param>
		/// <param name="predicates"></param>
		/// <returns></returns>
		public static IEnumerable<T> WhereAll<T>(this IEnumerable<T> enumeration, params Predicate<T>[] predicates) =>
			enumeration.Where((x) => predicates.All((predicate) => predicate(x)));

		/// <summary>
		/// Filters a sequence to elements for which all of <paramref name="predicates"/> return true.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumeration"></param>
		/// <param name="predicates"></param>
		/// <returns></returns>
		public static IEnumerable<T> WhereAll<T>(this IEnumerable<T> enumeration, IEnumerable<Predicate<T>> predicates) =>
			enumeration.WhereAll(predicates.ToArray());

		/// <summary>
		/// Filters a sequence to elements for which any of <paramref name="predicates"/> return true.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumeration"></param>
		/// <param name="predicates"></param>
		/// <returns></returns>
		public static IEnumerable<T> WhereAny<T>(this IEnumerable<T> enumeration, params Predicate<T>[] predicates) =>
			enumeration.Where((x) => predicates.Any((predicate) => predicate(x)));

		/// <summary>
		/// Filters a sequence to elements for which any of <paramref name="predicates"/> return true.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumeration"></param>
		/// <param name="predicates"></param>
		/// <returns></returns>
		public static IEnumerable<T> WhereAny<T>(this IEnumerable<T> enumeration, IEnumerable<Predicate<T>> predicates) =>
			enumeration.WhereAny(predicates.ToArray());

		/// <summary>
		/// Filters a sequence to elements for which <paramref name="predicate"/> return true.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumeration"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static IEnumerable<T> WherePredicate<T>(this IEnumerable<T> enumeration, Predicate<T> predicate) =>
			enumeration.Where((x) => predicate(x));

		/// <summary>
		/// Returns a sequence of integers from 0 to <paramref name="n"/> (excluding - last integer is <paramref name="n"/> - 1)
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static IEnumerable<int> ToSequence(this int n)
		{
			for(int i=0; i<n; ++i)
			{
				yield return i;
			}
		}

		#endregion
	}
}