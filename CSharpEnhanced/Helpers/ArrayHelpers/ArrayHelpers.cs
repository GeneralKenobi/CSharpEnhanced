namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Class with helper methods for arrays
	/// </summary>
	public static class ArrayHelpers
    {
		#region Public static methods

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
			for (int i = 0; i < array.Length; ++i)
			{
				for (int j = 0; j < array.Length; ++j)
				{
					array[i,j] = value;
				}
			}
		}

		#endregion
	}
}