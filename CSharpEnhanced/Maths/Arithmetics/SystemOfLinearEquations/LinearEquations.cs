// This file contains the LinearEquations class which is a base that exposes some properties that are used in every
// method of solving linear equations. Each method should be put in a seperate file that adds to the partial definition
// of this class. The implementations should expose static methods that use this base


using System.Numerics;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Solves systems of linear equations using the chosen method
	/// </summary>
	public partial class LinearEquations
	{
		#region Constructor

		/// <summary>
		/// Default Constructor, methods that use it should make sure that the requirements they present for these
		/// matrices are fulfilled
		/// </summary>
		protected LinearEquations(Complex[,] coefficients, Complex[] freeTerms)
		{
			// Assign the matrices
			_Coefficients = coefficients;
			_FreeTerms = freeTerms;

			// Create and fill the vector denoting variables
			_Variables = new int[_Size];

			for(int i=0; i<_Size; ++i)
			{
				_Variables[i] = i;
			}
		}

		#endregion

		#region Protected properties

		/// <summary>
		/// i-th cell contains index of a variable that is associated with i-th equation
		/// (it's important to keep track when swapping rows)
		/// </summary>
		protected int[] _Variables { get; }

		/// <summary>
		/// Matrix of coefficients
		/// </summary>
		protected Complex[,] _Coefficients { get; set; }

		/// <summary>
		/// Matrix of free terms
		/// </summary>
		protected Complex[] _FreeTerms { get; set; }

		/// <summary>
		/// The size of the system - number of equations
		/// </summary>
		protected int _Size => _FreeTerms.Length;

		#endregion
	}
}