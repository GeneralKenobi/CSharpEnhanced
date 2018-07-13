﻿using System;
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
		/// Adds respective entries from row <paramref name="rowToAdd"/> to entries in row <paramref name="rowToAddTo"/>
		/// </summary>
		/// <param name="rowToAddTo"></param>
		/// <param name="rowToAdd"></param>
		private void AddRowToRow(int rowToAddTo, int rowToAdd)
		{
			for (int i = 0; i < Columns; ++i)
			{
				Coefficients[rowToAddTo, i] = Coefficients[rowToAddTo, i].Add(Coefficients[rowToAdd, i]);
			}

			FreeTerms[rowToAddTo] = FreeTerms[rowToAddTo].Add(FreeTerms[rowToAdd]);
		}

		/// <summary>
		/// Adds respective entries from row <paramref name="rowToAdd"/> multiplied by <paramref name="multiplier"/>
		/// to entries in row <paramref name="rowToAddTo"/>
		/// </summary>
		/// <param name="rowToAddTo"></param>
		/// <param name="rowToAdd"></param>
		/// <param name="multiplier"></param>
		private void AddRowToRow(int rowToAddTo, int rowToAdd, IExpression multiplier)
		{
			for (int i = 0; i < Columns; ++i)
			{
				Coefficients[rowToAddTo, i] = Coefficients[rowToAddTo, i].Add(Coefficients[rowToAdd, i].Multiply(multiplier));
			}

			FreeTerms[rowToAddTo] = FreeTerms[rowToAddTo].Add(FreeTerms[rowToAdd].Multiply(multiplier));
		}

		/// <summary>
		/// Subtracts respective entries from row <paramref name="rowToSubtract"/> from entries in row
		/// <paramref name="rowToSubtractFrom"/>
		/// </summary>
		/// <param name="rowToSubtractFrom"></param>
		/// <param name="rowToSubtract"></param>
		private void SubtractRowFromRow(int rowToSubtractFrom, int rowToSubtract)
		{
			for (int i = 0; i < Columns; ++i)
			{
				Coefficients[rowToSubtractFrom, i] = Coefficients[rowToSubtractFrom, i].Subtract(Coefficients[rowToSubtract, i]);
			}

			FreeTerms[rowToSubtractFrom] = FreeTerms[rowToSubtractFrom].Subtract(FreeTerms[rowToSubtract]);
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

		/// <summary>
		/// Multiplies a row by the given number
		/// </summary>
		/// <param name="row"></param>
		/// <param name="number"></param>
		private void MultiplyRowBy(int row, IExpression number)
		{
			for (int i = 0; i < Columns; ++i)
			{
				Coefficients[row, i] = Coefficients[row,i].Multiply(number);
			}

			FreeTerms[row] = FreeTerms[row].Multiply(number);
		}

		/// <summary>
		/// Divides a row by the given number
		/// </summary>
		/// <param name="row"></param>
		/// <param name="number"></param>
		private void DivideRowBy(int row, IExpression number) => MultiplyRowBy(row, number.Reciprocal());

		private void DivideRowByDiagonal(int row)
		{
			var divider = Coefficients[row, row];

			for (int i = 0; i < Columns; ++i)
			{
				Coefficients[row, i] = Coefficients[row, i].Divide(divider);
			}

			FreeTerms[row] = FreeTerms[row].Divide(divider);
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

			for (int i = 0; i < Columns; ++i)
			{
				temp = Coefficients[row1, i];
				Coefficients[row1, i] = Coefficients[row2, i];
				Coefficients[row2, i] = temp;
			}
		}
	}
}