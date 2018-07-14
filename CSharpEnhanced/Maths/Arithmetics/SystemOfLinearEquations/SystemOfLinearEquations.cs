using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace CSharpEnhanced.Maths
{
	public class SystemOfLinearEquations
	{
		/// <summary>
		/// Matrix of coefficients
		/// </summary>
		public IExpression[,] Coefficients { get; set; }

		/// <summary>
		/// Matrix of free terms
		/// </summary>
		public IExpression[] FreeTerms { get; set; }


		public int Rows => FreeTerms.Length;

		public int Columns => Coefficients.GetLength(1);

		/// <summary>
		/// Solves the system with use of partial gauss elimination
		/// </summary>
		public void Solve()
		{
			ForwardElemination();
			BackwardsElimination();
		}

		private void ForwardElemination()
		{
			for (int i = 0; i < Rows; ++i)
			{
				DivideRowByDiagonal(i);

				for (int j = i + 1; j < Rows; ++j)
				{
					SubtractRows(j, i, Coefficients[j, i], i + 1, Columns);
				}
			}
		}

		private void BackwardsElimination()
		{
			for (int i = Rows - 1; i >= 0; --i)
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

			for (int i = row; i < Columns; ++i)
			{
				Coefficients[row, i] = Coefficients[row, i].Divide(divider);
			}

			FreeTerms[row] = FreeTerms[row].Divide(divider);
		}
	}
}