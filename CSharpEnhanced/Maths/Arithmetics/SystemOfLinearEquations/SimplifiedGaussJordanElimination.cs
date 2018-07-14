using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpEnhanced.Maths
{
    public partial class LinearEquations
    {
		/// <summary>
		/// Solves the system with use of partial gauss elimination
		/// </summary>
		public static IExpression[] SimplifiedGaussJordanElimination(IExpression[,] coefficients, IExpression[] freeTerms)
		{
			// Check for null
			if(coefficients == null || freeTerms == null)
			{
				throw new ArgumentNullException();
			}

			// Check if the dimensions match
			if (coefficients.GetLength(0) == coefficients.GetLength(1) &&
				coefficients.GetLength(1) == freeTerms.Length)
			{
				// Create a new solver instance
				var solver = new SimplifiedGaussJordanEliminationSolver(coefficients, freeTerms);
				
				// Perform necessary operations
				solver.ForwardElemination();
				solver.BackwardsElimination();

				// Return the result
				return solver.FreeTerms;
			}
			else
			{
				throw new ArgumentException(nameof(coefficients) + " is not a square matrix or its size does not" +
					" equal the length of " + nameof(freeTerms));
			}
		}

		private class SimplifiedGaussJordanEliminationSolver : LinearEquations
		{

			/// <summary>
			/// Default Constructor
			/// </summary>
			public SimplifiedGaussJordanEliminationSolver(IExpression[,] coefficients, IExpression[] freeTerms)
				: base(coefficients, freeTerms)	{ }

			public void ForwardElemination()
			{
				for (int i = 0; i < Size; ++i)
				{
					DivideRowByDiagonal(i);

					for (int j = i + 1; j < Size; ++j)
					{
						SubtractRows(j, i, Coefficients[j, i], i + 1, Size);
					}
				}
			}

			public void BackwardsElimination()
			{
				for (int i = Size - 1; i >= 0; --i)
				{
					for (int j = i - 1; j >= 0; --j)
					{
						SubtractFreeTerms(j, i, Coefficients[j, i]);
					}
				}
			}

			private void SubtractRows(int rowToSubtractFrom, int rowToSubtract, IExpression multiplier,
				int startFromColumn, int endAtColumn)
			{
				for (int i = startFromColumn; i < endAtColumn; ++i)
				{
					Coefficients[rowToSubtractFrom, i] =
						Coefficients[rowToSubtractFrom, i].Subtract(Coefficients[rowToSubtract, i].Multiply(multiplier));
				}

				FreeTerms[rowToSubtractFrom] = FreeTerms[rowToSubtractFrom].Subtract(FreeTerms[rowToSubtract].Multiply(multiplier));
			}

			private void SubtractFreeTerms(int rowToSubtractFrom, int rowToSubtract, IExpression multiplier) =>
				FreeTerms[rowToSubtractFrom] = FreeTerms[rowToSubtractFrom].Subtract(FreeTerms[rowToSubtract].Multiply(multiplier));

			private void DivideRowByDiagonal(int row)
			{
				var divider = Coefficients[row, row];

				for (int i = row; i < Size; ++i)
				{
					Coefficients[row, i] = Coefficients[row, i].Divide(divider);
				}

				FreeTerms[row] = FreeTerms[row].Divide(divider);
			}
		}
	}
}
