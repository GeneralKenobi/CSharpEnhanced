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
	}
}
