using System;
using System.Collections.Generic;

namespace CSharpEnhanced.CoreClasses
{
	/// <summary>
	/// Equality comparer that can be initialized with Funcs for both <see cref="Equals(T, T)"/> and <see cref="GetHashCode(T)"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CustomEqualityComparer<T> : EqualityComparer<T>
	{
		#region Constructors

		/// <summary>
		/// Default constructor, for both <see cref="Equals(T, T)"/> and <see cref="GetHashCode(T)"/> default methods provided by
		/// <see cref="object"/> class will be used
		/// </summary>
		public CustomEqualityComparer() { }

		/// <summary>
		/// <paramref name="equalsFunc"/> will be used for <see cref="Equals(T, T)"/> and <see cref="GetHashCode(T)"/> will use the
		/// default method provided by <see cref="object"/>
		/// </summary>
		public CustomEqualityComparer(Func<T, T, bool> equalsFunc)
		{
			_Equals = equalsFunc;
		}

		/// <summary>
		/// <paramref name="getHashCodeFunc"/> will be used for <see cref="GetHashCode(T)"/> and <see cref="Equals(T, T)"/> will use
		/// the default method provided by <see cref="object"/>
		/// </summary>
		public CustomEqualityComparer(Func<T, int> getHashCodeFunc)
		{
			_GetHashCode = getHashCodeFunc;
		}

		/// <summary>
		/// <paramref name="equalsFunc"/> will be used for <see cref="Equals(T, T)"/> and <see cref="GetHashCode(T)"/> will be used
		/// for <see cref="GetHashCode(T)"/>
		/// </summary>
		public CustomEqualityComparer(Func<T, T, bool> equalsFunc, Func<T, int> getHashCodeFunc)
		{
			_Equals = equalsFunc;
			_GetHashCode = getHashCodeFunc;
		}

		#endregion

		#region Private properties

		/// <summary>
		/// Func performing equality comparison
		/// </summary>
		Func<T, T, bool> _Equals { get; }

		/// <summary>
		/// Func performing hash code computation
		/// </summary>
		Func<T, int> _GetHashCode { get; }

		#endregion

		#region Public methods

		/// <summary>
		/// Returns true if <paramref name="x"/> and <paramref name="y"/> are equal according to the Func passed in constructor or,
		/// if no Func was passed, to the <see cref="object.Equals(object)"/> method
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public override bool Equals(T x, T y) => _Equals?.Invoke(x, y) ?? x.Equals(y);

		/// <summary>
		/// Returns the hash code of <paramref name="obj"/> computed using the Func passed in constructor or, if no Func was passed,
		/// to the <see cref="object.GetHashCode"/>
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override int GetHashCode(T obj) => _GetHashCode?.Invoke(obj) ?? obj.GetHashCode();

		#endregion
	}
}