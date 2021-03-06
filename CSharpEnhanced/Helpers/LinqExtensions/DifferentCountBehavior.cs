﻿namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Defines possible behaviors for when count of sequences in
	/// <see cref="LinqExtensions.MergeSelect{T1, T2, TResult}(System.Collections.Generic.IEnumerable{T1}, System.Collections.Generic.IEnumerable{T2}, System.Func{T1, T2, TResult})"/>
	/// is different
	/// </summary>
	public enum DifferentCountBehavior
    {
		/// <summary>
		/// Exception is thrown
		/// </summary>
		ThrowException = 0,

		/// <summary>
		/// Empty enumeration is returned
		/// </summary>
		ReturnEmpty = 1,

		/// <summary>
		/// Only the beginning of the longer sequence is taken, the number of taken elements corresponds to the number of elements
		/// in the shorter sequence. Example: for an adding func, sequences a,b,c,d and x,y,z will return ax,by,cz.
		/// </summary>
		TakeBeginningOfLonger = 2,

		/// <summary>
		/// Only the ending of the longer sequence is taken, the number of taken elements corresponds to the number of elements
		/// in the shorter sequence. Example: for an adding func, sequences a,b,c,d and x,y,z will return bx,cy,dz.
		/// </summary>
		TakeEndingOfLonger = 3,
	}
}