using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Extension methods for <see cref="Complex"/>
	/// </summary>
    public static class ComplexExtensionMethods
    {
		/// <summary>
		/// Returns a complex number whose components are rounded to the nearest multiple of <paramref name="roundTo"/>
		/// </summary>
		/// <param name="complex">Complex number to round</param>
		/// <param name="roundTo">Number whose multiple to round to</param>
		/// <returns></returns>
		public static Complex RoundTo(this Complex complex, double roundTo,
			MidpointRounding rounding = MidpointRounding.AwayFromZero) =>
			new Complex(Math.Round(complex.Real / roundTo, rounding) * roundTo,
				Math.Round(complex.Imaginary / roundTo, rounding) * roundTo);
	}
}
