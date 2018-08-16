using System;

namespace CSharpEnhanced.CoreClasses
{
	/// <summary>
	/// Flags that denote modifier keys that may have been present with a key press
	/// </summary>
	[Flags]
	public enum KeyModifiers
    {
		/// <summary>
		/// No modifiers are present
		/// </summary>
		None = 0,

		/// <summary>
		/// Ctrl key
		/// </summary>
		Ctrl = 1,

		/// <summary>
		/// Alt key
		/// </summary>
		Alt = 2,

		/// <summary>
		/// Shift key
		/// </summary>
		Shift = 4,
    }
}