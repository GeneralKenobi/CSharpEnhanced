using CSharpEnhanced.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpEnhanced.CoreClasses
{
	/// <summary>
	/// Simulates a switch statement based on a type
	/// </summary>
	public class TypeSwitch
	{
		#region Private properties

		/// <summary>
		/// Dictionary with actions for types types that match strictly (eg. inherited classes won't match parent class)
		/// </summary>
		private Dictionary<Type, Action<object>> _StrictlyMatchingActions { get; } = new Dictionary<Type, Action<object>>();

		/// <summary>
		/// Dictionary with actions for types that match strictly (eg. inherited classes will match parent class)
		/// </summary>
		private Dictionary<Type, Action<object>> _LazyMatchingActions { get; } = new Dictionary<Type, Action<object>>();

		#endregion

		#region Public methods

		/// <summary>
		/// Adds a new strict case to the switch - objects whose type equals (comparison using equality comparator)
		/// <typeparamref name="T"/> will invoke the action (eg. deriving types won't match for <typeparamref name="T"/> that is
		/// their parent)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="action"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public TypeSwitch StrictCase<T>(Action<T> action)
		{
			if(action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			_StrictlyMatchingActions.Add(typeof(T), (x) => action((T)x));
			return this;
		}

		/// <summary>
		/// Adds a new lazy case to the switch - objects whose type can be assigned to (comparison using
		/// <see cref="Type.IsAssignableFrom(Type)"/> method) will invoke the action (eg. deriving types will match for
		/// <typeparamref name="T"/> that is their parent)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="action"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public TypeSwitch LazyCase<T>(Action<T> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			_LazyMatchingActions.Add(typeof(T), (x) => action((T)x));
			return this;
		}

		/// <summary>
		/// Switches on <paramref name="obj"/>'s type, invokes matching actions with <paramref name="obj"/> casted to type that was
		/// used as a key for that action.
		/// </summary>
		/// <param name="obj"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public void Switch(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			// Get the strictly matching types (use equality comparator)
			_StrictlyMatchingActions.Where((x) => obj.GetType() == x.Key).
			// Concat them with lazily matching types (those that obj's type can be assigned to)
			Concat(_LazyMatchingActions.Where((x) => x.Key.IsAssignableFrom(obj.GetType()))).
			// And perform each resulting action
			ForEach((x) => x.Value(obj));
		}

		#endregion
	}
}