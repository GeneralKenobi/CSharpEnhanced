using System;

namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Class with helper methods for arrays
	/// </summary>
	public static class ArrayHelpers
    {
		#region Public static methods

		#region Initialization

		/// <summary>
		/// Creates a one-dimensional array of length <paramref name="length"/> and initializes it with the <paramref name="value"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static T[] CreateAndInitialize<T>(T value, int length)
		{
			var array = new T[length];

			array.Initialize(value);
			
			return array;
		}

		/// <summary>
		/// Initializes the array with <paramref name="value"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="value"></param>
		public static void Initialize<T>(this T[] array, T value)
		{
			for (int i = 0; i < array.Length; ++i)
			{
				array[i] = value;
			}
		}

		/// <summary>
		/// Creates a two-dimensional array of length <paramref name="length1"/> and <paramref name="length2"/>,
		/// and initializes it with the <paramref name="value"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static T[,] CreateAndInitialize<T>(T value, int length1, int length2)
		{
			var array = new T[length1, length2];

			array.Initialize(value);

			return array;
		}

		/// <summary>
		/// Initializes the array with <paramref name="value"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="value"></param>
		public static void Initialize<T>(this T[,] array, T value)
		{
			for (int i = 0; i < array.GetLength(0); ++i)
			{
				for (int j = 0; j < array.GetLength(1); ++j)
				{
					array[i,j] = value;
				}
			}
		}

		#endregion

		#region Copying to/from 2d arrays

		/// <summary>
		/// Copies vector <paramref name="source"/> into the chosen row of <paramref name="destination"/>.
		/// Number of columns in <paramref name="destination"/> should match the length of <paramref name="source"/>.
		/// <paramref name="rowIndex"/> should be smaller than the number of rows in <paramref name="destination"/>.
		/// Method performs a shallow copy.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="destination"></param>
		/// <param name="source"></param>
		/// <param name="rowIndex">Indexing starts at 0</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static void CopyRowInto<T>(this T[,] destination, T[] source, int rowIndex)
		{
			// Check if rowIndex does not exceed the number of rows in destination
			if (rowIndex >= destination.GetLength(0))
			{
				throw new ArgumentOutOfRangeException(nameof(rowIndex));
			}

			// Check if number of columns in destination matches the number of elements in source
			if(destination.GetLength(1) != source.Length)
			{
				throw new ArgumentException($"Number of columns in {nameof(destination)} does not match the length of {source}");
			}

			// Copy elements, one by one
			for (int i = 0; i < source.Length; ++i)
			{
				destination[rowIndex, i] = source[i];
			}
		}

		/// <summary>
		/// Copies and returns a row from <paramref name="source"/>.
		/// <paramref name="rowIndex"/> should be smaller than the number of rows in <paramref name="destination"/>.
		/// Method performs a shallow copy.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="rowIndex">Indexing starts at 0</param>
		/// <returns>Return value is a 1d array with length equal to number of columns in <paramref name="source"/></returns>
		public static T[] CopyRowFrom<T>(this T[,] source, int rowIndex)
		{
			// Check if rowIndex does not exceed the number of rows in destination
			if (rowIndex >= source.GetLength(0))
			{
				throw new ArgumentOutOfRangeException(nameof(rowIndex));
			}

			// Create array for result
			var result = new T[source.GetLength(1)];

			// Copy elements into it, one by one
			for(int i = 0; i < result.Length; ++i)
			{
				result[i] = source[rowIndex, i];
			}

			// Return the result
			return result;
		}

		#endregion

		#endregion
	}
}