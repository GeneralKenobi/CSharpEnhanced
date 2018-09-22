using System;
using System.Reflection;

namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Class containing methods related to types
	/// </summary>
	public static class TypeHelpers
    {
		#region Public static methods

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

		/// <summary>
		/// Tries to cast <paramref name="obj"/> to type <typeparamref name="T"/> using explicit cast
		/// and returns true on success.
		/// Saves eithe the casted <paramref name="obj"/> or <see cref="default(T)"/> to <paramref name="casted"/>.
		/// </summary>
		/// <typeparam name="T">Type to cast to</typeparam>
		/// <param name="obj">Object to cast</param>
		/// <returns>True on success, false on failure</returns>
		public static bool TryCast<T>(object obj, out T casted)
		{
			try
			{
				casted = (T)obj;
				return true;
			}
			catch(InvalidCastException)
			{
				casted = default(T);
				return false;
			}
		}

		/// <summary>
		/// Returns true if the type can be constructed without using any parameters (for example using <see cref="Activator"/>).
		/// Checks whether the type is not an interface or is not abstract and if it has a parameterless constructor.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool CanBeConstructedWithoutParameters(this Type type) =>
			// Check if there's a parameterless constructor and if the type is not abstract or an interface
			type.GetConstructor(Type.EmptyTypes) != null && !(type.IsAbstract || type.IsInterface);

		/// <summary>
		/// Tries to get property from some type, returns true if successful and assigns the value to <paramref name="property"/>,
		/// returns false if unsuccessful and assigns null to <paramref name="property"/>.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="propertyName"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static bool TryGetProperty(this Type type, string propertyName, out PropertyInfo property)
		{
			// Get the property
			property = type.GetProperty(propertyName);

			// The action was successful if it did not return null
			return property != null;
		}

		/// <summary>
		/// Tries to get property from some type, returns true if successful and assigns the value to <paramref name="property"/>,
		/// returns false if unsuccessful and assigns null to <paramref name="property"/>. If the property has a type different than
		/// <typeparamref name="T"/> the action will be considered unsuccessful.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="propertyName"></param>
		/// <param name="property"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static bool TryGetProperty<T>(this Type type, string propertyName, out PropertyInfo property)
		{
			// Get the property
			property = type.GetProperty(propertyName);

			// Check if types match
			if(property?.PropertyType != typeof(T))
			{
				// If not, assign null to property (action was unsuccessful)
				property = null;
			}

			// The action was successful if it did not return null
			return property != null;
		}

		#endregion
	}
}