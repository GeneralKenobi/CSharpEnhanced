namespace CSharpEnhanced.CoreClasses
{
	/// <summary>
	/// Wraps a key event into a class containing a string representation of the key and key modifiers present during key press
	/// </summary>
	public class KeyArgument
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public KeyArgument(string key, KeyModifiers modifiers)
		{
			Key = key;
			Modifiers = modifiers;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Key that was pressed
		/// </summary>
		public string Key { get; }

		/// <summary>
		/// Modifiers active 
		/// </summary>
		public KeyModifiers Modifiers { get; }

		#endregion
	}
}