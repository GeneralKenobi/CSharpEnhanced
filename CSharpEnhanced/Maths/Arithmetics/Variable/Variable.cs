using CSharpEnhanced.CoreClasses;
using System.Numerics;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Class representing a single variable that may be referenced by many objects
	/// </summary>
	public class Variable : RefWrapper<Complex>
    {
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public Variable() { }

		/// <summary>
		/// Default Constructor
		/// </summary>
		public Variable(Complex value)
		{
			Value = value;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// True if the imaginary component is equal to 0
		/// </summary>
		public bool IsPureReal => Value.Imaginary == 0;

		/// <summary>
		/// True if the real component is equal to 0
		/// </summary>
		public bool IsPureImaginary => Value.Real == 0;

		#endregion

		#region Public static properties

		/// <summary>
		/// Pure real one
		/// </summary>
		public static Variable One { get; } = new Variable(Complex.One);
		
		/// <summary>
		/// Pure real negative one
		/// </summary>
		public static Variable NegativeOne { get; } = new Variable(-Complex.One);

		/// <summary>
		/// Zero
		/// </summary>
		public static Variable Zero { get; } = new Variable(Complex.Zero);

		/// <summary>
		/// Pure imaginary one
		/// </summary>
		public static Variable ImaginaryOne { get; } = new Variable(Complex.ImaginaryOne);

		/// <summary>
		/// Pure imaginary negative one
		/// </summary>
		public static Variable NegativeImaginaryOne { get; } = new Variable(-Complex.ImaginaryOne);

		#endregion
	}
}