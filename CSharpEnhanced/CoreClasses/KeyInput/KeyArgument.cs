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
		public KeyArgument(string key, KeyModifiers modifiers = KeyModifiers.None)
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

		#region Public methods

		/// <summary>
		/// Compares two <see cref="KeyArgument"/>s, if both public properties are equal then the objects are equal.
		/// Keys are not case sensitive.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is KeyArgument shortcut)
			{
				return Key.ToUpper() == shortcut.Key.ToUpper() && Modifiers == shortcut.Modifiers;
			}

			return false;
		}

		/// <summary>
		/// Returns a unique hash code for the instance
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() => Key.ToUpper().GetHashCode() * 17 + Modifiers.GetHashCode();

		#endregion

		#region Public static properties

		/// <summary>
		/// Empty key argument
		/// </summary>
		public static KeyArgument Empty { get; } = new KeyArgument(string.Empty);

		#endregion
	}
}