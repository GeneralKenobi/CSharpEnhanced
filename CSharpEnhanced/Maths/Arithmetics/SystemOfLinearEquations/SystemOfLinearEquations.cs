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
			for (int i = 0; i < Rows; ++i)
			{
				DivideRowByDiagonal(i);
				
				for(int j = i + 1; j < Rows; ++j)
				{
					SubtractRowFromRow(j, i, Coefficients[j,i]);					
				}
			}
			
			for (int i = Rows - 1; i >= 0; --i)
			{
				for (int j = i - 1; j >= 0; --j)
				{
					SubtractRowFromRow(j, i, Coefficients[j,i]);
				}
			}
		}

		/// <summary>
		/// Subtracts respective entries from row <paramref name="rowToSubtract"/> multiplied by <paramref name="multiplier"/>
		/// from entries in row <paramref name="rowToSubtractFrom"/>
		/// </summary>
		/// <param name="rowToSubtractFrom"></param>
		/// <param name="rowToSubtract"></param>
		/// <param name="multiplier"></param>
		private void SubtractRowFromRow(int rowToSubtractFrom, int rowToSubtract, IExpression multiplier)
		{
			for (int i = 0; i < Columns; ++i)
			{
				Coefficients[rowToSubtractFrom, i] =
					Coefficients[rowToSubtractFrom, i].Subtract(Coefficients[rowToSubtract, i].Multiply(multiplier));
			}

			FreeTerms[rowToSubtractFrom] = FreeTerms[rowToSubtractFrom].Subtract(FreeTerms[rowToSubtract].Multiply(multiplier));
		}

		private void DivideRowByDiagonal(int row)
		{
			var divider = Coefficients[row, row];

			for (int i = 0; i < Columns; ++i)
			{
				Coefficients[row, i] = Coefficients[row, i].Divide(divider);
			}

			FreeTerms[row] = FreeTerms[row].Divide(divider);
		}
	}
}