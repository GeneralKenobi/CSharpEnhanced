using System;
using System.Collections.Generic;
using System.Linq;
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

			if (value < 1e-3 || value > 1e+3)
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
		/// Returns a string representation with SI unit included with the most fitting prefix
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="useFullName"></param>
		/// <returns></returns>
		public static string ToSIString(double value, string unit, bool useFullName = false) =>
			// Get the closest prefix
			ToSIString(value, unit, GetClosestPrefix(value), useFullName);

		/// <summary>
		/// Returns a string representation with SI unit included with the given prefix
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="prefix"></param>
		/// <param name="useFullName"></param>
		/// <returns></returns>
		public static string ToSIString(double value, string unit, SIPrefix prefix, bool useFullName = false)
		{
			// Modify the value to match it
			value /= Math.Pow(10, prefix.Base10Power);

			// Return it plus prefix name (or symbol) and unit
			return value.ToString() + (useFullName ? prefix.Name : prefix.Symbol) + unit;
		}

		/// <summary>
		/// Returns a string representation with SI unit included with the most fitting prefix, excluding prefixes with Base10Power
		/// absolute value smaller than 3 (exception: 0)
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <param name="useFullName"></param>
		/// <returns></returns>
		public static string ToSIStringExcludingSmallPrefixes(double value, string unit, bool useFullName = false) =>
			// Get the closes prefix
			ToSIString(value, unit, GetClosestPrefixExcludingSmall(value), useFullName);

		#endregion
	}
}