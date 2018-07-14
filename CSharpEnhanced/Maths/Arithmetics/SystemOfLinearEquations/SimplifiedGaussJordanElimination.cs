using System;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Solves systems of linear equations using the chosen method
	/// </summary>
	public partial class LinearEquations
    {
		#region Public static methods

		/// <summary>
		/// Solves a system of linear equations expected to have a unique solution
		/// using a simplified Gauss-Jordan elimination (only the operations on matrix of coefficients that are
		/// necessary to obtain the result are performed). The matrix of coefficients and the vector of free
		/// terms are both modified in the process.
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
				solver.Solve();

				// Return the result
				return solver.FreeTerms;
			}
			else
			{
				throw new ArgumentException(nameof(coefficients) + " is not a square matrix or its size does not" +
					" equal the length of " + nameof(freeTerms));
			}
		}

		#endregion

		#region Helper class

		/// <summary>
		/// Helper class that can be used to solve a system of linear equations expected to have a unique solution
		/// using a simplified Gauss-Jordan elimination (only the operations on matrix of coefficients that are
		/// necessary to obtain the result are performed). The matrix of coefficients and the vector of free
		/// terms are both modified in the process.
		/// </summary>
		private class SimplifiedGaussJordanEliminationSolver : LinearEquations
		{
			#region Constructor

			/// <summary>
			/// Default Constructor
			/// </summary>
			public SimplifiedGaussJordanEliminationSolver(IExpression[,] coefficients, IExpression[] freeTerms)
				: base(coefficients, freeTerms)	{ }

			#endregion

			#region Public methods

			/// <summary>
			/// Solves the system (result is the vector of FreeTerms)
			/// </summary>
			public void Solve()
			{
				ForwardElemination();
				BackwardsElimination();
			}

			#endregion

			#region Private methods

			/// <summary>
			/// Performs forward elimination on the system - matrix is brought into the reduced lower-triangular form
			/// (all entries on the main diagonal are 1, all entries below the main diagonal are "virtually" 0
			/// meaning that the performed operations would result in them being zero but they're never used later on
			/// so the actual operation is not performed on them)
			/// </summary>
			private void ForwardElemination()
			{
				// For each row
				for (int i = 0; i < Size; ++i)
				{
					// Divide it by diagonal entry
					DivideRowByDiagonal(i);

					// For each column that is to the right of the diagonal entry (that still has non-zero entry)
					// subtract the entries in corresponding rows (multiplier is chosen to obtain 0 below the
					// entry on the main diagonal)
					for (int j = i + 1; j < Size; ++j)
					{
						SubtractRows(j, i, Coefficients[j, i], i + 1, Size);
					}
				}
			}

			/// <summary>
			/// Performs backwards elimination on the matrix. To give a correct result has to start from a reduced
			/// lower-triangular for. To increase efficiency the calculations are only actually performed for
			/// the vector of free terms since the resulting manipulation of matrix of coefficients is not important
			/// anymore.
			/// </summary>
			private void BackwardsElimination()
			{
				// Starting from the bottom for each row
				for (int i = Size - 1; i >= 0; --i)
				{
					// Choose a coefficient that would result in a 0 in each entry above the entry on the main diagonal
					// and subtract free terms only (no need to modify matrix of coefficients - increased efficiency)
					for (int j = i - 1; j >= 0; --j)
					{
						SubtractFreeTerms(j, i, Coefficients[j, i]);
					}
				}
			}

			/// <summary>
			/// Subtracts entries on two rows according to the given parameters
			/// </summary>
			/// <param name="rowToSubtractFrom">Row to subtract from</param>
			/// <param name="rowToSubtract">Row to subtract</param>
			/// <param name="multiplier"><see cref="IExpression"/> by which each subtracted entry is multiplied</param>
			/// <param name="startFromColumn">Starts the operation from the column indexed by this value</param>
			/// <param name="endAtColumn">Ends the operation at a column indexed by this value</param>
			private void SubtractRows(int rowToSubtractFrom, int rowToSubtract, IExpression multiplier,
				int startFromColumn, int endAtColumn)
			{
				// For each chosen column
				for (int i = startFromColumn; i < endAtColumn; ++i)
				{
					// Subtract the values
					Coefficients[rowToSubtractFrom, i] =
						Coefficients[rowToSubtractFrom, i].Subtract(Coefficients[rowToSubtract, i].Multiply(multiplier));
				}

				// Subtract the free terms
				FreeTerms[rowToSubtractFrom] = FreeTerms[rowToSubtractFrom].Subtract(FreeTerms[rowToSubtract].Multiply(multiplier));
			}

			/// <summary>
			/// Subtracts free terms of two rows
			/// </summary>
			/// <param name="rowToSubtractFrom">Row to subtract from</param>
			/// <param name="rowToSubtract">Row to subtract</param>
			/// <param name="multiplier"><see cref="IExpression"/> by which each subtracted entry is multiplied</param>
			private void SubtractFreeTerms(int rowToSubtractFrom, int rowToSubtract, IExpression multiplier) =>
				FreeTerms[rowToSubtractFrom] = FreeTerms[rowToSubtractFrom].Subtract(FreeTerms[rowToSubtract].Multiply(multiplier));

			/// <summary>
			/// Divides all elements to the right of the main diagonal (including) as well as the free term
			/// of the given row by the entry on the main diagonal
			/// </summary>
			/// <param name="row"></param>
			private void DivideRowByDiagonal(int row)
			{
				// Get the divider (entry on the main diagonal)
				var divider = Coefficients[row, row];
				
				// Divide each element in the row by it
				for (int i = row; i < Size; ++i)
				{
					Coefficients[row, i] = Coefficients[row, i].Divide(divider);
				}
				
				// Also divide the free term by the entry
				FreeTerms[row] = FreeTerms[row].Divide(divider);
			}

			#endregion
		}

		#endregion
	}
}