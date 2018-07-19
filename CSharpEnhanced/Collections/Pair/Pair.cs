using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpEnhanced.Collections
{
	/// <summary>
	/// Class containg two elements, similiar to <see cref="Tuple{T1, T2}"/> but is mutable and serves only as a wrapper, for
	/// example for a Queue of pairs
	/// </summary>
	public class Pair<T1, T2>
    {
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public Pair() { }
		
		/// <summary>
		/// Constructor with parameters
		/// </summary>
		public Pair(T1 first, T2 second)
		{
			First = first;
			Second = second;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// First item in the pair
		/// </summary>
		public T1 First { get; set; }

		/// <summary>
		/// Second item in the pair
		/// </summary>
		public T2 Second { get; set; }

		#endregion
	}
}
