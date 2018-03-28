using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Class containing methods related to types
	/// </summary>
    public static class TypeHelpers
    {
		/// <summary>
		/// Returns true if <paramref name="toCheck"/> either derives from <paramref name="baseType"/> or is <paramref name="baseType"/>
		/// </summary>
		/// <param name="toCheck"></param>
		/// <param name="baseType"></param>
		/// <returns></returns>
		public static bool IsDerivedOrSelf(this Type toCheck, Type baseType) => toCheck.IsSubclassOf(baseType) || toCheck == baseType;

		/// <summary>
		/// Tries to convert the string literal to a number type (in the following order: int, double, long). Returns true on success
		/// </summary>
		/// <param name="str">String that may be a number</param>
		/// <returns></returns>
		public static bool TryParseNumber(string str, out object result)
		{
			// Try to convert to in
			if(int.TryParse(str, out int i))
			{
				result = i;
			}
			
			// Otherwise try to convert to double
			else if(double.TryParse(str, out double d))
			{
				result = d;
			}

			// Otherwise try to convert to long
			else if(long.TryParse(str, out long l))
			{
				result = l;
			}

			// If nothing succeeded assign null and return false
			else
			{
				result = null;
				return false;
			}

			// If there was a success then return true
			return true;
		}
	}
}
