using System;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Class containing helper methods
	/// </summary>
	public static class MathsHelpers
    {
		#region Public methods

		/// <summary>
		/// Reduces unnecessary turns so that the angle is in the first turn. Returns the corrected value
		/// </summary>
		/// <param name="angle">Angle to reduce</param>
		/// <param name="unit">unit the angle is denoted in, default is radians</param>
		/// <returns></returns>
		public static double ReduceAngle(double angle, AngleUnit unit = AngleUnit.Radians)
		{
			double fullTurn = 1;

			// Assign a full turn value dependent on the unit type
			switch(unit)
			{
				case AngleUnit.Radians:
					{
						fullTurn = 2 * Math.PI;
					}
					break;

				case AngleUnit.Degrees:
					{
						fullTurn = 360;
					}
					break;

				case AngleUnit.Turns:
					{
						fullTurn = 1;
					}
					break;
			}

			if (angle >= fullTurn)
			{
				// For positive phase greater than first turn
				return angle - fullTurn * Math.Floor(angle / fullTurn);
			}
			else if (angle < 0)
			{
				// For negative phase
				return angle - fullTurn * (Math.Ceiling(angle / fullTurn) - 1);
			}
			else
			{
				// For a single turn
				return angle;
			}
		}

		/// <summary>
		/// Returns a double number rounded to the nearest multiple of <paramref name="roundTo"/>
		/// </summary>
		/// <param name="value">Number to round</param>
		/// <param name="roundTo">Number whose multiple to round to</param>
		/// <param name="rounding"></param>
		/// <returns></returns>
		public static double RoundTo(this double value, double roundTo, MidpointRounding rounding = MidpointRounding.AwayFromZero) =>
			Math.Round(value / roundTo, rounding) * roundTo;

		/// <summary>
		/// Returns a double number rounded down to the nearest multiple of <paramref name="floorTo"/>
		/// </summary>
		/// <param name="value">Number to round down</param>
		/// <param name="floorTo">Number whose multiple to round down to</param>
		/// <returns></returns>
		public static double FloorTo(this double value, double floorTo) => Math.Floor(value / floorTo) * floorTo;

		/// <summary>
		/// Returns a double number rounded up to the nearest multiple of <paramref name="ceilingTo"/>
		/// </summary>
		/// <param name="value">Number to round up</param>
		/// <param name="ceilingTo">Number whose multiple to round up to</param>
		/// <returns></returns>
		public static double CeilingTo(this double value, double ceilingTo) => Math.Ceiling(value / ceilingTo) * ceilingTo;

		#endregion
	}
}