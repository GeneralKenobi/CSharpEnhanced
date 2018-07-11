using CSharpEnhanced.CoreClasses;
using System.Numerics;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Class representing a single variable that may be referenced by many objects
	/// </summary>
	public class Variable : RefWrapper<Complex>
    {
		public bool IsPureReal => Value.Imaginary == 0;

		public bool IsPureImaginary => Value.Real == 0;
    }
}