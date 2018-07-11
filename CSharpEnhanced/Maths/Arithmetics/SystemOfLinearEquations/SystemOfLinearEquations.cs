using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CSharpEnhanced.Maths
{
    public class SystemOfLinearEquations
    {
		public Expression[,] Coefficients { get; set; }
		public Expression[] FreeTerms { get; set; }


		public int Rows => FreeTerms.Length;

		public int Columns => Coefficients.GetLength(1);

		/// <summary>
		/// Solves the system with use of partial gauss elimination
		/// </summary>
		public void Solve()
		{
			for(int i=0; i<Rows; ++i)
			{
				DivideRowBy()
			}
		}

		/// <summary>
		/// Adds respective entries from row <paramref name="rowToAdd"/> to entries in row <paramref name="rowToAddTo"/>
		/// </summary>
		/// <param name="rowToAddTo"></param>
		/// <param name="rowToAdd"></param>
		private void AddRowToRow(int rowToAddTo, int rowToAdd) => AddRowToRow(rowToAddTo, rowToAdd, Complex.One);

		/// <summary>
		/// Adds respective entries from row <paramref name="rowToAdd"/> multiplied by <paramref name="multiplier"/>
		/// to entries in row <paramref name="rowToAddTo"/>
		/// </summary>
		/// <param name="rowToAddTo"></param>
		/// <param name="rowToAdd"></param>
		/// <param name="multiplier"></param>
		private void AddRowToRow(int rowToAddTo, int rowToAdd, Complex multiplier)
		{
			for(int i = 0; i< Columns; ++i)
			{
				Coefficients[rowToAddTo, i] += Coefficients[rowToAdd, i] * multiplier;
			}

			FreeTerms[rowToAddTo] += FreeTerms[rowToAdd] * multiplier;
		}

		/// <summary>
		/// Subtracts respective entries from row <paramref name="rowToSubtract"/> from entries in row
		/// <paramref name="rowToSubtractFrom"/>
		/// </summary>
		/// <param name="rowToSubtractFrom"></param>
		/// <param name="rowToSubtract"></param>
		private void SubtractRowFromRow(int rowToSubtractFrom, int rowToSubtract) =>
			AddRowToRow(rowToSubtractFrom, rowToSubtract, -Complex.One);

		/// <summary>
		/// Subtracts respective entries from row <paramref name="rowToSubtract"/> multiplied by <paramref name="multiplier"/>
		/// from entries in row <paramref name="rowToSubtractFrom"/>
		/// </summary>
		/// <param name="rowToSubtractFrom"></param>
		/// <param name="rowToSubtract"></param>
		/// <param name="multiplier"></param>
		private void SubtractRowFromRow(int rowToSubtractFrom, int rowToSubtract, Complex multiplier) =>
			AddRowToRow(rowToSubtractFrom, rowToSubtract, -multiplier);

		/// <summary>
		/// Multiplies a row by the given number
		/// </summary>
		/// <param name="row"></param>
		/// <param name="number"></param>
		private void MultiplyRowBy(int row, Complex number)
		{
			for (int i = 0; i < Columns; ++i)
			{
				Coefficients[row,i] *= number;
			}

			FreeTerms[row] *= number;
		}

		/// <summary>
		/// Divides a row by the given number
		/// </summary>
		/// <param name="row"></param>
		/// <param name="number"></param>
		private void DivideRowBy(int row, Complex number) => MultiplyRowBy(row, Complex.Reciprocal(number));

		private void DivideRowByDiagonal(int row)
		{
			for (int i = 0; i < Columns; ++i)
			{
				Coefficients[row, i] /= Coefficients[row,row];
			}

			FreeTerms[row] *= number;
		}

		/// <summary>
		/// Swaps two rows
		/// </summary>
		/// <param name="row1"></param>
		/// <param name="row2"></param>
		private void SwapRows(int row1, int row2)
		{
			var temp = FreeTerms[row1];
			FreeTerms[row1] = FreeTerms[row2];
			FreeTerms[row2] = temp;

			for(int i=0; i<Columns; ++i)
			{
				temp = Coefficients[row1,i];
				Coefficients[row1, i] = Coefficients[row2, i];
				Coefficients[row2, i] = temp;
			}
		}
	}
}