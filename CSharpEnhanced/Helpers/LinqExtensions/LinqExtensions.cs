using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Class containing extension methods related to <see cref="IEnumerable{T}"/> and Linq.
	/// </summary>
	public static class LinqExtensions
	{
		#region Private static methods

		/// <summary>
		/// Helper of MergeSelect methods, checks whether counts of sequences match the provided <paramref name="differentCountBehavior"/>
		/// behavior, returns true if opertation may be performed, false if it may not be performed and an empty enumerator should be
		/// returned.
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="s1Enum"></param>
		/// <param name="s2Enum"></param>
		/// <param name="s1Count"></param>
		/// <param name="s2Count"></param>
		/// <param name="differentCountBehavior"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		private static bool MergeSelectCountCheck(IEnumerator s1Enum, IEnumerator s2Enum, int s1Count, int s2Count,
			DifferentCountBehavior differentCountBehavior)
		{
			// Check whether count is correct, if not take appropriate action
			if (s1Count != s2Count)
			{
				switch (differentCountBehavior)
				{
					case DifferentCountBehavior.ThrowException:
						{
							throw new ArgumentException("Sequences must have the same elements count");
						}

					case DifferentCountBehavior.ReturnEmpty:
						{
							return false;
						}

					// Adjust the enumerator of the longer sequence so that enumeration will end for both sequences at the same time
					case DifferentCountBehavior.TakeEndingOfLonger:
						{
							if (s1Count > s2Count)
							{
								s1Enum.MoveNext(s1Count - s2Count);
							}
							else
							{
								s2Enum.MoveNext(s2Count - s1Count);
							}
						}
						break;
				}
			}

			return true;
		}

		#endregion

		#region Public static methods

		#region Concatenation

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
		/// Concats <paramref name="items"/> with <paramref name="enumeration"/>
		/// </summary>
		/// <typeparam name="T">Type of enumeration</typeparam>
		/// <param name="enumeration">Enumeration to concat to</param>
		/// <param name="items">Items to concat</param>
		/// <returns></returns>
		public static IEnumerable<T> ConcatAtBeginning<T>(this IEnumerable<T> enumeration, IEnumerable<T> items) =>
			// Check if enumeration isnt' null, cast items to IEnumerable<T> to use the linq method (otherwise recursion would occur)
			items.Concat(enumeration ?? throw new ArgumentNullException(nameof(enumeration)));

		#endregion

		#region Inline for each loop

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

		#endregion

		#region Where methods

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

		#endregion

		#region To sequence

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

		#region Merge select

		/// <summary>
		/// Projects two sequences into one sequence. Uses <paramref name="selectFunc"/> to transform pairs of elements (one element
		/// is taken from <paramref name="s1"/> and one is taken from <paramref name="s2"/>) into a single element.
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <param name="selectFunc"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static IEnumerable<TResult> MergeSelect<T1, T2, TResult>(this IEnumerable<T1> s1, IEnumerable<T2> s2,
			Func<T1, T2, TResult> selectFunc, DifferentCountBehavior differentCountBehavior = DifferentCountBehavior.ThrowException)
		{
			// Check if data is correct
			if (s1 == null) throw new ArgumentNullException(nameof(s1));
			if (s2 == null) throw new ArgumentNullException(nameof(s2));
			if (selectFunc == null) throw new ArgumentNullException(nameof(selectFunc));

			// Get enumerators
			var s1Enum = s1.GetEnumerator();
			var s2Enum = s2.GetEnumerator();

			// Check whether count is correct, if not take appropriate action
			if(!MergeSelectCountCheck(s1Enum, s2Enum, s1.Count(), s2.Count(), differentCountBehavior))
			{
				yield break;
			}

			// Go through each pair, return the result of the func on each
			while(s1Enum.MoveNext() && s2Enum.MoveNext())
			{
				yield return selectFunc(s1Enum.Current, s2Enum.Current);
			}
		}

		/// <summary>
		/// Projects two sequences into one sequence. Uses <paramref name="selectFunc"/> to transform pairs of elements (one element
		/// is taken from <paramref name="s1"/> and one is taken from <paramref name="s2"/>) into a single element. Provides an
		/// iterating variable to the func.
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <param name="selectFunc"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static IEnumerable<TResult> MergeSelect<T1, T2, TResult>(this IEnumerable<T1> s1, IEnumerable<T2> s2,
			Func<T1, T2, int, TResult> selectFunc, DifferentCountBehavior differentCountBehavior = DifferentCountBehavior.ThrowException)
		{
			// Check if data is correct
			if (s1 == null) throw new ArgumentNullException(nameof(s1));
			if (s2 == null) throw new ArgumentNullException(nameof(s2));
			if (selectFunc == null) throw new ArgumentNullException(nameof(selectFunc));

			// Get enumerators
			var s1Enum = s1.GetEnumerator();
			var s2Enum = s2.GetEnumerator();

			// Check whether count is correct, if not take appropriate action
			if (!MergeSelectCountCheck(s1Enum, s2Enum, s1.Count(), s2.Count(), differentCountBehavior))
			{
				yield break;
			}

			int i = 0;

			// Go through each pair, return the result of the func on each
			while (s1Enum.MoveNext() && s2Enum.MoveNext())
			{
				yield return selectFunc(s1Enum.Current, s2Enum.Current, i++);
			}
		}

		#endregion

		#region Sequence equality comparison

		/// <summary>
		/// Returns true if both sequences contain exactly the same elements - both count and individual elements are the same, order is not
		/// important.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static bool IsSequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second) =>
			// Sequences are equal if both first / second and second / first are empty sequences - both Any methods return false which is
			// then negated to true. If at least one Any returns true then sum is true and final result is false.
			!(first.Except(second).Any() || second.Except(first).Any());

		#endregion

		#endregion
	}
}