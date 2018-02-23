using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CSharpEnhanced.CoreClasses
{
	/// <summary>
	/// Base class for classes that don't derive from anything and want to implement <see cref="INotifyPropertyChanged"/>
	/// </summary>
	class INotifyPropertyChangedImplemented : INotifyPropertyChanged
	{
		#region Property Changed Event

		/// <summary>
		/// Property Changed Event
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Public methods

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
	}
}
