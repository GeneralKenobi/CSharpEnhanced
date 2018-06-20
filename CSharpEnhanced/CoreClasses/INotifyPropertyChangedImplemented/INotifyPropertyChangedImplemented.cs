using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEnhanced.CoreClasses
{
	/// <summary>
	/// Base class for classes that don't derive from anything and want to implement <see cref="INotifyPropertyChanged"/>
	/// </summary>
	public class INotifyPropertyChangedImplemented : INotifyPropertyChanged
	{
		#region Property Changed Event

		/// <summary>
		/// Property Changed Event
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Property Changed Invoke

		/// <summary>
		/// Invokes Property Changed Event for each string parameter
		/// </summary>
		/// <param name="propertyName">Properties to invoke for. Null or string.Empty will result in notification for all properties</param>
		public void InvokePropertyChanged(params string[] propertyNames)
		{
			foreach (var item in propertyNames)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(item));
			}
		}

		#endregion

		#region Delayed On Property Changed

		/// <summary>
		/// Fires property changed event for the given property after the delay
		/// </summary>
		/// <param name="propertyNames">Names of the property to notify for</param>
		/// <param name="delay">Delay before the property changed is fired, in ms (0-10000)</param>
		/// <returns></returns>
		public async Task InvokePropertyChangedDelayed(int delay, params string[] propertyNames)
		{
			// Check for the minimum value
			if (delay < MinimumDelay)
			{
				delay = MinimumDelay;
			}

			// Check for the maximum value
			if (delay > MaximumDelay)
			{
				delay = MaximumDelay;
			}

			// Await the delay
			await Task.Delay(delay);

			// Fire the property changed event
			InvokePropertyChanged(propertyNames);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Minimum delay for <see cref="InvokePropertyChangedDelayed(int, string[])"
		/// </summary>
		public static int MinimumDelay => 0;

		/// <summary>
		/// Maximum delay for <see cref="InvokePropertyChangedDelayed(int, string[])"/>
		/// </summary>
		public static int MaximumDelay => 10000;

		#endregion
	}
}
