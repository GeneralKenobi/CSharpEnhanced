using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using CSharpEnhanced.Maths;

namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Class containing helper methods connected with SI - The International System of Units
	/// </summary>
	public static class SIHelpers
    {
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		static SIHelpers()
		{
			_MinimumPrefixPower = _Prefixes.Min((prefix) => prefix.Base10Power);
			_MaximumPrefixPower = _Prefixes.Max((prefix) => prefix.Base10Power);
			_EmptyPrefix = _Prefixes.Find((prefix) => prefix.Base10Power == 0);
		}

		#endregion

		#region Private static properties

		/// <summary>
		/// The minimum power for which a prefix is defined
		/// </summary>
		private static int _MinimumPrefixPower { get; }

		/// <summary>
		/// The maximum power for which a prefix is defined
		/// </summary>
		private static int _MaximumPrefixPower { get; }

		/// <summary>
		/// Empty prefix which in fact does not change the value
		/// </summary>
		private static SIPrefix _EmptyPrefix { get; }

		/// <summary>
		/// Backing store for <see cref="Prefixes"/>; contains all prefixed defined in SI system
		/// </summary>
		private static List<SIPrefix> _Prefixes { get; } = new List<SIPrefix>()
		{
			new SIPrefix("yocto", "y", -24),
			new SIPrefix("zepto", "z", -21),
			new SIPrefix("atto", "a", -18),
			new SIPrefix("femto", "f", -15),
			new SIPrefix("pico", "p", -12),
			new SIPrefix("nano", "n", -9),
			new SIPrefix("micro", "µ", -6),
			new SIPrefix("milli", "m", -3),
			new SIPrefix("centi", "c", -2),
			new SIPrefix("deci", "d", -1),
			new SIPrefix(string.Empty, string.Empty, 0),
			new SIPrefix("deca", "da", 1),
			new SIPrefix("hecto", "h", 2),
			new SIPrefix("kilo", "k", 3),
			new SIPrefix("mega", "M", 6),
			new SIPrefix("giga", "G", 9),
			new SIPrefix("tera", "T", 12),
			new SIPrefix("peta", "P", 15),
			new SIPrefix("exa", "E", 18),
			new SIPrefix("zetta", "Z", 21),
			new SIPrefix("yotta", "Y", 24),
		};

		#endregion

		#region Public static properties

		/// <summary>
		/// Contains all prefixes defined in the SI standard
		/// </summary>
		public static IEnumerable<SIPrefix> Prefixes => new List<SIPrefix>(_Prefixes);

		#endregion

		#region Public static methods

		#region Get prefix methods

		/// <summary>
		/// Returns a prefix matching the given base10 power
		/// </summary>
		/// <param name="base10Power">The power to which 10 is raised in a single value to have the prefix (eg. 3 for kilo)</param>
		/// <returns></returns>
		public static SIPrefix GetPrefix(int base10Power) => _Prefixes.Find((prefix) => prefix.Base10Power == base10Power);

		/// <summary>
		/// Gets the most appropriate prefix, does not take into account prefixes with Base10Power absolute value smaller than 3
		/// (exception: 0)
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SIPrefix GetClosestPrefixExcludingSmall(double value)
		{
			value = Math.Abs(value);

			if (value <= 1e-3 || value >= 1e+3)
			{
				return GetClosestPrefix(value);
			}

			if (value < 1)
			{
				return GetPrefix(-3);
			}

			return GetPrefix(0);
		}

		/// <summary>
		/// Gets the most appropriate prefix
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SIPrefix GetClosestPrefix(double value)
		{
			// If the value is 0 return no prefix instead of the smallest one
			if(value == 0)
			{
				return GetPrefix(0);
			}

			// Take the logarithm of the value to find the necessary power for 10 (floor it to have the result with at least 1 in
			// front of the unit)
			var base10Power = Math.Floor(Math.Log10(Math.Abs(value)));

			// Make sure the power does not exceed the defined range (i.e. it's greater than or equal to the smallest defined
			// and smaller than or equal to the greatest defined)
			base10Power = Math.Max(base10Power, _MinimumPrefixPower);
			base10Power = Math.Min(base10Power, _MaximumPrefixPower);

			// If the absolute value of power is greater than or equal to 3 it's necessary to round it down to a multiple of 3 because
			// prefixes are defined in order ...10^-9, 10^-6, 10^-3, 10^-2, 10^-1, 10^0, 10^1, 10^2, 10^3, 10^6, 10^9...
			return GetPrefix(Math.Abs(base10Power) <= 3 ? (int)base10Power : (int)base10Power.FloorTo(3));
		}

		/// <summary>
		/// Gets the most appropriate prefix (searches according to the greatest absolute value component of the <see cref="Complex"/>
		/// number), does not take into account prefixes with Base10Power absolute value smaller than 3 (exception: 0)
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SIPrefix GetClosestPrefixExcludingSmall(Complex value) =>
			GetClosestPrefixExcludingSmall(Math.Max(Math.Abs(value.Real), Math.Abs(value.Imaginary)));

		/// <summary>
		/// Gets the most appropriate prefix (searches according to the greatest absolute value component of the <see cref="Complex"/>
		/// number)
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SIPrefix GetClosestPrefix(Complex value) =>
			GetClosestPrefix(Math.Max(Math.Abs(value.Real), Math.Abs(value.Imaginary)));

		#endregion

		#region ToSIString for doubles

		/// <summary>
		/// Returns a string representation with SI unit included with the most fitting prefix
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="roundToDigit">Digit to round the value to. If it's smaller than or equal to 0, no rounding will be done</param>
		/// <param name="useFullName"></param>
		/// <returns></returns>
		public static string ToSIString(double value, string unit = "", int roundToDigit = 0,
			MidpointRounding midpointRounding = MidpointRounding.AwayFromZero, bool useFullName = false) =>
			// Get the closest prefix
			ToSIString(value, GetClosestPrefix(value), unit, roundToDigit, midpointRounding, useFullName);

		/// <summary>
		/// Returns a string representation with SI unit included with the given prefix
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="prefix"></param>
		/// <param name="roundToDigit">Digit to round the value to. If it's smaller than or equal to 0, no rounding will be done</param>
		/// <param name="useFullName"></param>
		/// <returns></returns>
		public static string ToSIString(double value, SIPrefix prefix, string unit = "", int roundToDigit = 0,
			MidpointRounding midpointRounding = MidpointRounding.AwayFromZero, bool useFullName = false)
		{
			// Modify the value to match it
			value /= Math.Pow(10, prefix.Base10Power);

			// If roundToDigit is greater than 0, round the value to a specific digit
			if(roundToDigit > 0)
			{
				value = value.RoundToDigit(roundToDigit, midpointRounding);
			}

			// Return it plus prefix name (or symbol) and unit
			return value.ToString() + (useFullName ? prefix.Name : prefix.Symbol) + unit;
		}

		/// <summary>
		/// Returns a string representation with SI unit included with the most fitting prefix, excluding prefixes with Base10Power
		/// absolute value smaller than 3 (exception: 0)
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="roundToDigit">Digit to round the value to. If it's smaller than or equal to 0, no rounding will be done</param>
		/// <param name="useFullName"></param>
		/// <returns></returns>
		public static string ToSIStringExcludingSmallPrefixes(double value, string unit = "", int roundToDigit = 0,
			MidpointRounding midpointRounding = MidpointRounding.AwayFromZero, bool useFullName = false) =>
			// Get the closes prefix
			ToSIString(value, GetClosestPrefixExcludingSmall(value), unit, roundToDigit, midpointRounding,useFullName);

		#endregion

		#region ToSIString for Complex numbers

		/// <summary>
		/// Returns a string representation with SI unit included with the most fitting prefix, excluding prefixes with Base10Power
		/// absolute value smaller than 3 (exception: 0)
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="useFullName"></param>
		/// <returns></returns>
		public static string ToSIStringExcludingSmallPrefixes(Complex value, string unit = "", int roundToDigit = 0,
			MidpointRounding midpointRounding = MidpointRounding.AwayFromZero, bool useFullName = false) =>
			// Get the closes prefix
			ToSIString(value, GetClosestPrefixExcludingSmall(value), unit, roundToDigit, midpointRounding, useFullName);

		/// <summary>
		/// Returns a string representation with SI unit included with the most fitting prefix
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="useFullName"></param>
		/// <returns></returns>
		public static string ToSIString(Complex value, string unit = "", int roundToDigit = 0,
			MidpointRounding midpointRounding = MidpointRounding.AwayFromZero, bool useFullName = false) =>
			// Get the closest prefix
			ToSIString(value, GetClosestPrefix(value), unit, roundToDigit, midpointRounding, useFullName);

		/// <summary>
		/// Returns a string representation with SI unit included with the given prefix
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="prefix"></param>
		/// <param name="useFullName"></param>
		/// <returns></returns>
		public static string ToSIString(Complex value, SIPrefix prefix, string unit = "", int roundToDigit = 0,
			MidpointRounding midpointRounding = MidpointRounding.AwayFromZero, bool useFullName = false)
		{
			// Modify the value to match it
			value /= Math.Pow(10, prefix.Base10Power);

			// If roundToDigit is greater than 0, round the value to a specific digit
			if (roundToDigit > 0)
			{
				value = value.RoundToDigit(roundToDigit, midpointRounding);
			}

			// Return it plus prefix name (or symbol) and unit
			return value.ToString() + (useFullName ? prefix.Name : prefix.Symbol) + unit;
		}

		/// <summary>
		/// Returns a string representation with SI unit included with the most fitting prefix, excluding prefixes with Base10Power
		/// absolute value smaller than 3 (exception: 0). Alternative number complex number string representation is given as a sum
		/// instead of a coordinate on the complex plane, eg: (24+5i)kV
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="useFullName"></param>
		/// <param name="imaginaryAsJ">If true, "j" is used as imaginary unit instead of "i"</param>
		/// <returns></returns>
		public static string ToAltSIStringExcludingSmallPrefixes(Complex value, string unit = "", int roundToDigit = 0,
			MidpointRounding midpointRounding = MidpointRounding.AwayFromZero, bool useFullName = false, bool imaginaryAsJ = false) =>
			// Get the closes prefix
			ToAltSIString(value, GetClosestPrefixExcludingSmall(value), unit, roundToDigit, midpointRounding, useFullName, imaginaryAsJ);

		/// <summary>
		/// Returns a string representation with SI unit included with the most fitting prefix. Alternative number complex number
		/// string representation is given as a sum instead of a coordinate on the complex plane, eg: (24+5i)kV
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="useFullName"></param>
		/// <param name="imaginaryAsJ">If true, "j" is used as imaginary unit instead of "i"</param>
		/// <returns></returns>
		public static string ToAltSIString(Complex value, string unit = "", int roundToDigit = 0,
			MidpointRounding midpointRounding = MidpointRounding.AwayFromZero, bool useFullName = false, bool imaginaryAsJ = false) =>
			// Get the closest prefix
			ToAltSIString(value, GetClosestPrefix(value), unit, roundToDigit, midpointRounding, useFullName, imaginaryAsJ);

		/// <summary>
		/// Returns a string representation with SI unit included with the given prefix. Alternative number complex number string
		/// representation is given as a sum instead of a coordinate on the complex plane, eg: (24+5i)kV
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="prefix"></param>
		/// <param name="useFullName"></param>
		/// <param name="imaginaryAsJ">If true, "j" is used as imaginary unit instead of "i"</param>
		/// <returns></returns>
		public static string ToAltSIString(Complex value, SIPrefix prefix, string unit = "", int roundToDigit = 0,
			MidpointRounding midpointRounding = MidpointRounding.AwayFromZero, bool useFullName = false, bool imaginaryAsJ = false)
		{
			// Modify the value to match it
			value /= Math.Pow(10, prefix.Base10Power);

			// If roundToDigit is greater than 0, round the value to a specific digit
			if (roundToDigit > 0)
			{
				value = value.RoundToDigit(roundToDigit, midpointRounding);
			}

			// If value is 0 return "0" plus the unit
			if (value == 0)
			{
				return "0" + unit;
			}
						
			string result = string.Empty;

			// If real part is not 0
			if(value.Real != 0)
			{
				// Add its string version to the result
				result += value.Real.ToString();
			}

			// If imaginary is not 0
			if(value.Imaginary!=0)
			{
				// And it's nonnegative and the real part wasn't 0
				if(value.Imaginary >= 0 && value.Real!=0)
				{
					// Include a plus sign
					result += "+";
				}

				// Add the imaginary part as string to the result with the imaginary unit
				result += value.Imaginary.ToString() + (imaginaryAsJ ? "j" : "i");

				// Wrap the whole thing in brackets
				result = "(" + result + ")";
			}

			// Finish it off with unit + prefix
			result += (useFullName ? prefix.Name : prefix.Symbol) + unit;

			return result;
		}

		#endregion

		#region SIString parsing

		/// <summary>
		/// Parses the string as an SI string, returns the value on success or throws an exception on failure. Everything behind the
		/// prefix is considered to be the unit (eg. for "11.31kadwadaw" the "k" will be considered the prefix and "adwadaw" will be
		/// considered the unit, eg. for  22.415adwadaw "adwadaw" will again be considered as the unit (because the prefix is not
		/// present))
		/// </summary>
		/// <param name="s"></param>
		/// <param name="result"></param>
		/// <param name="simplifiedMicro">If true when determining prefix "u" will be considered as "µ"</param>
		/// <returns></returns>
		public static double ParseSIString(string s, bool simplifiedMicro = true)
		{
			if (TryParseSIString(s, out double result))
			{
				return result;
			}

			throw new ArgumentException(nameof(s) + " is not an SI string");
		}

		/// <summary>
		/// Tries to parse the string as an SI string, returns true on success (and assigns the parsed value to result) and false
		/// on failure (assigns 0 to result). Everything behind the prefix is considered to be the unit (eg. for "11.31kadwadaw"
		/// the "k" will be considered the prefix and "adwadaw" will be considered the unit, eg. for  22.415adwadaw "adwadaw" will
		/// again be considered as the unit (because the prefix is not present))
		/// </summary>
		/// <param name="s"></param>
		/// <param name="result"></param>
		/// <param name="simplifiedMicro">If true when determining prefix "u" will be considered as "µ"</param>
		/// <returns></returns>
		public static bool TryParseSIString(string s, out double result, bool simplifiedMicro = true)
		{
			// Assign default result
			result = 0;
			
			// Check for null
			if (s == null)
			{
				return false;
			}

			// Remove white spaces
			s = s.Replace(" ", string.Empty);

			// Swap the commas to dots to standardize the input
			s = s.Replace(",", ".");

			// If simplified micro symbol is enabled replace all u with µ
			if (simplifiedMicro)
			{
				s = s.Replace("u", "µ");
			}

			// Get the index of the last digit
			var lastDigitIndex = s.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

			// Tro to get the prefix (excluding the empty prefix)
			var prefix = _Prefixes.Find((entry) => entry != _EmptyPrefix &&
				(s.IndexOf(entry.Symbol) == lastDigitIndex + 1 || s.IndexOf(entry.Name) == lastDigitIndex + 1));

			// Check if it was found
			if (prefix == null)
			{
				// If not, assign the empty prefix
				prefix = _EmptyPrefix;
			}

			// If the prefix symbol/name is not on the beginning of the unit string then it was a false posotive or if,
			// due to manipulations, the string became empty (no numbers) return failure
			if (string.IsNullOrWhiteSpace(s))
			{
				return false;
			}

			// Remove everything that is after the last digit (make sure not to pass an index equal to or greater to length)
			if(lastDigitIndex != s.Length - 1)
			{
				s = s.Remove(lastDigitIndex + 1);
			}
			
			// Try to parse the result
			if(double.TryParse(s, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
				CultureInfo.InvariantCulture, out result))
			{
				// If successful, multiply the result by a 10 with proper power
				result *= Math.Pow(10, prefix.Base10Power);
				return true;
			}

			// The string is not an SIString
			return false;
		}

		#endregion

		#endregion
	}
}