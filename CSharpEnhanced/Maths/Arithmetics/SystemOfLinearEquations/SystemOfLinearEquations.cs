//using System;
//using System.Collections.Generic;
//using System.Numerics;
//using System.Text;

//namespace CSharpEnhanced.Maths
//{
//    public class SystemOfLinearEquations
//    {
//		/// <summary>
//		/// Matrix of coefficients
//		/// </summary>
//		public Expression[,] Coefficients { get; set; }

//		/// <summary>
//		/// Matrix of free terms
//		/// </summary>
//		public Expression[] FreeTerms { get; set; }


//		public int Rows => FreeTerms.Length;

//		public int Columns => Coefficients.GetLength(1);

//		/// <summary>
//		/// Solves the system with use of partial gauss elimination
//		/// </summary>
//		public void Solve()
//		{
//			for(int i=0; i<Rows; ++i)
//			{
//				DivideRowByDiagonal(i);
//			}
//		}

//		/// <summary>
//		/// Adds respective entries from row <paramref name="rowToAdd"/> to entries in row <paramref name="rowToAddTo"/>
//		/// </summary>
//		/// <param name="rowToAddTo"></param>
//		/// <param name="rowToAdd"></param>
//		private void AddRowToRow(int rowToAddTo, int rowToAdd) => AddRowToRow(rowToAddTo, rowToAdd, Variable.One);

//		/// <summary>
//		/// Adds respective entries from row <paramref name="rowToAdd"/> multiplied by <paramref name="multiplier"/>
//		/// to entries in row <paramref name="rowToAddTo"/>
//		/// </summary>
//		/// <param name="rowToAddTo"></param>
//		/// <param name="rowToAdd"></param>
//		/// <param name="multiplier"></param>
//		private void AddRowToRow(int rowToAddTo, int rowToAdd, Product multiplier)
//		{
//			for(int i = 0; i< Columns; ++i)
//			{
//				Coefficients[rowToAddTo, i] += Coefficients[rowToAdd, i] * multiplier;
//			}

//			FreeTerms[rowToAddTo] += FreeTerms[rowToAdd] * multiplier;
//		}

//		/// <summary>
//		/// Subtracts respective entries from row <paramref name="rowToSubtract"/> from entries in row
//		/// <paramref name="rowToSubtractFrom"/>
//		/// </summary>
//		/// <param name="rowToSubtractFrom"></param>
//		/// <param name="rowToSubtract"></param>
//		private void SubtractRowFromRow(int rowToSubtractFrom, int rowToSubtract) =>
//			AddRowToRow(rowToSubtractFrom, rowToSubtract, Variable.NegativeOne);

//		/// <summary>
//		/// Subtracts respective entries from row <paramref name="rowToSubtract"/> multiplied by <paramref name="multiplier"/>
//		/// from entries in row <paramref name="rowToSubtractFrom"/>
//		/// </summary>
//		/// <param name="rowToSubtractFrom"></param>
//		/// <param name="rowToSubtract"></param>
//		/// <param name="multiplier"></param>
//		private void SubtractRowFromRow(int rowToSubtractFrom, int rowToSubtract, Product multiplier) =>
//			AddRowToRow(rowToSubtractFrom, rowToSubtract, -multiplier);

//		/// <summary>
//		/// Multiplies a row by the given number
//		/// </summary>
//		/// <param name="row"></param>
//		/// <param name="number"></param>
//		private void MultiplyRowBy(int row, Product number)
//		{
//			for (int i = 0; i < Columns; ++i)
//			{
//				Coefficients[row,i] *= number;
//			}

//			FreeTerms[row] *= number;
//		}

//		/// <summary>
//		/// Divides a row by the given number
//		/// </summary>
//		/// <param name="row"></param>
//		/// <param name="number"></param>
//		private void DivideRowBy(int row, Product number) => MultiplyRowBy(row, number.Reciprocal());

//		private void DivideRowByDiagonal(int row)
//		{
//			var divider = Coefficients[row, row];

//			for (int i = 0; i < Columns; ++i)
//			{
//				Coefficients[row, i] /= divider;
//			}

//			FreeTerms[row] /= divider;
//		}

//		/// <summary>
//		/// Swaps two rows
//		/// </summary>
//		/// <param name="row1"></param>
//		/// <param name="row2"></param>
//		private void SwapRows(int row1, int row2)
//		{
//			var temp = FreeTerms[row1];
//			FreeTerms[row1] = FreeTerms[row2];
//			FreeTerms[row2] = temp;

//			for(int i=0; i<Columns; ++i)
//			{
//				temp = Coefficients[row1,i];
//				Coefficients[row1, i] = Coefficients[row2, i];
//				Coefficients[row2, i] = temp;
//			}
//		}
//	}
//}