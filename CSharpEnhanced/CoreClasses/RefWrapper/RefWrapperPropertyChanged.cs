using System.ComponentModel;

namespace CSharpEnhanced.CoreClasses
{
	/// <summary>
	/// Class that may be used to wrap non-reference classes to be able to store them by reference that also implements
	/// <see cref="INotifyPropertyChanged"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RefWrapperPropertyChanged<T> : INotifyPropertyChanged
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public RefWrapperPropertyChanged() { }

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		public RefWrapperPropertyChanged(T value)
		{
			Value = value;
		}

		#endregion
		
		#region Events

		/// <summary>
		/// Event fired whenever a property changes its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Private properties

		/// <summary>
		/// Backing store for <see cref="Value"/>
		/// </summary>
		private T _Value { get; set; }

		#endregion

		#region Public properties

		/// <summary>
		/// Stored value
		/// </summary>
		public T Value
		{
			get => _Value;
			set
			{
				if(!_Value.Equals(value))
				{
					_Value = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
				}
			}
		}

		#endregion
	}
}