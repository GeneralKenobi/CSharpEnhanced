using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Class containing helper methods connected with properties
	/// </summary>
    public static class PropertyHelpers
    {
		/// <summary>
		/// Returns the value of the given property on the given object. Throws an exception if the object was null or the
		/// property wasn't found.
		/// </summary>
		/// <param name="obj">Object to search</param>
		/// <param name="property">Name of the property to whose value to retrieve</param>
		/// <returns></returns>
		public static object GetPropertyValue(object obj, string property)
		{
			if(obj == null)
			{
				throw new NullReferenceException(nameof(obj));
			}

			var propertyInfo = obj.GetType().GetProperty(property);

			// Check if the property was retrieved properly
			if(propertyInfo != null)
			{
				return propertyInfo.GetValue(obj);
			}
			else
			{
				// If not, throw an exception
				throw new Exception("Public property \"" + property + "\" wasn't found on " + nameof(obj));
			}
		}

		/// <summary>
		/// Retrieves the value of the given property on the given object and stores it in <paramref name="value"/>.
		/// Returns true on success, on failure returns false and assigns null to <paramref name="value"/>
		/// </summary>
		/// <param name="obj">Object to search</param>
		/// <param name="property">Name of the property whose value to retrieve</param>
		/// <param name="value">Variable to store the value in</param>
		/// <returns></returns>
		public static bool TryGetPropertyValue(object obj, string property, out object value)
		{
			if (obj == null)
			{
				value = null;
				return false;
			}

			var propertyInfo = obj.GetType().GetProperty(property);

			// Check if the property was retrieved properly
			if (propertyInfo != null)
			{
				// If so, get its value and return true
				value = propertyInfo.GetValue(obj);
				return true;
			}
			else
			{
				// Otherwise assign null and return false
				value = null;
				return false;
			}
		}

		/// <summary>
		/// Sets the given property on the given object. Throws an exception if there were problems before calling
		/// <see cref="PropertyInfo.SetValue(object, object)"/>
		/// </summary>
		/// <param name="obj">Object to set the property on</param>
		/// <param name="property">Property to set</param>
		/// <param name="value">Value to assign</param>
		public static void SetPropertyValue(object obj, string property, object value)
		{
			if (obj == null)
			{
				throw new NullReferenceException(nameof(obj));
			}

			var propertyInfo = obj.GetType().GetProperty(property);

			// Check if the property was retrieved properly
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(obj, value);
			}
			else
			{
				// If not, throw an exception
				throw new Exception("Public property \"" + property + "\" wasn't found on " + nameof(obj));
			}
		}

		/// <summary>
		/// Sets the given property on the given object. Returns false if there were problems before calling
		/// <see cref="PropertyInfo.SetValue(object, object)"/>, true otherwise (success)
		/// </summary>
		/// <param name="obj">Object to set the property on</param>
		/// <param name="property">Property to set</param>
		/// <param name="value">Value to assign</param>
		public static bool TrySetPropertyValue(object obj, string property, object value)
		{
			if (obj == null)
			{
				return false;
			}

			var propertyInfo = obj.GetType().GetProperty(property);

			// Check if the property was retrieved properly
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(obj, value);
				return true;
			}
			else
			{
				// If not, return false
				return false;
			}
		}
	}
}
