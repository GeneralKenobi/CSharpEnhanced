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
			Coefficients = coefficients;
			FreeTerms = freeTerms;
		}

		#endregion

		#region Protected properties

		/// <summary>
		/// Matrix of coefficients
		/// </summary>
		protected Complex[,] Coefficients { get; set; }

		/// <summary>
		/// Matrix of free terms
		/// </summary>
		protected Complex[] FreeTerms { get; set; }

		/// <summary>
		/// The size of the system - number of equations
		/// </summary>
		protected int Size => FreeTerms.Length;

		#endregion
	}
}