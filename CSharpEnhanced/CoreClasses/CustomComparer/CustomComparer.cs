using System;
using System.Collections.Generic;

namespace CSharpEnhanced.CoreClasses
{
	/// <summary>
	/// <see cref="IComparer{T}"/> that uses a Func given as parameter in constructor for comparison.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CustomComparer<T> : Comparer<T>
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="comparisonFunc"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public CustomComparer(Func<T, T, int> comparisonFunc)
		{
			_ComparisonFunc = comparisonFunc ?? throw new ArgumentNullException(nameof(comparisonFunc));
		}

		#endregion

		#region Private properties

		/// <summary>
		/// Func used for comparison
		/// </summary>
		private Func<T, T, int> _ComparisonFunc { get; }

		#endregion

		#region Public methods

		/// <summary>
		/// Compares two objects using the func given in constructor
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public override int Compare(T x, T y) => _ComparisonFunc.Invoke(x, y);

		#endregion
	}
}