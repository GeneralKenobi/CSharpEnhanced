using System.Numerics;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Helper extension methods for <see cref="IExpression"/>
	/// </summary>
	public static class IExpressionHelpers
    {
		#region Public static methods

		/// <summary>
		/// Evaluates the array and returns it
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public static Complex[] Evaluate(this IExpression[] array)
		{
			var result = new Complex[array.Length];

			for(int i=0; i<array.Length; ++i)
			{
				result[i] = array[i].Evaluate();
			}

			return result;
		}

		/// <summary>
		/// Evaluates the array and returns it
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public static Complex[,] Evaluate(this IExpression[,] array)
		{
			var result = new Complex[array.GetLength(0), array.GetLength(1)];

			for (int i = 0; i < array.Length; ++i)
			{
				for (int j = 0; j < array.Length; ++j)
				{
					result[i, j] = array[i, j].Evaluate();
				}
			}

			return result;
		}

		#endregion
	}
}