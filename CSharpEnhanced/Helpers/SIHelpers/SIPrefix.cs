namespace CSharpEnhanced.Helpers
{
	/// <summary>
	/// Class containing information about a single prefix defined in SI
	/// </summary>
	public class SIPrefix
    {
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		internal SIPrefix(string name, string symbol, int base10Power)
		{
			Name = name;
			Symbol = symbol;
			Base10Power = base10Power;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Full name of the prefix (eg. kilo)
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Symbol of the prefix (eg. k)
		/// </summary>
		public string Symbol { get; }

		/// <summary>
		/// The power to which 10 is raised in a single value to have the prefix (eg. 3 for kilo)
		/// </summary>
		public int Base10Power { get; }

		#endregion
	}
}