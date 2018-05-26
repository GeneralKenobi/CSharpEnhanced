using System;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Class containing helper methods
	/// </summary>
	public static class Helpers
    {
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
    }
}