using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Class containing helper methods
	/// </summary>
	public static class MathsHelpers
    {
		#region Private static methods

		/// <summary>
		/// Helper of <see cref="CalculateMidPoints(double, double, int)"/>, <paramref name="pointsCount"/> is assumed to be
		/// greater than 1. Returns <paramref name="pointsCount"/> number of evenly spaced points between <paramref name="first"/> and
		/// <paramref name="last"/> (first is <paramref name="first"/> and last is <paramref name="last"/>)
		/// </summary>
		/// <param name="first"></param>
		/// <param name="last"></param>
		/// <param name="pointsCount"></param>
		/// <returns></returns>
		private static IEnumerable<double> CalculateMidPointsHelper(double first, double last, int pointsCount)
		{
			// Get the step between two subsequent points
			var step = (last - first) / (pointsCount - 1);

			// Start at first and then add i times the step
			for(int i=0; i<pointsCount; ++i)
			{
				yield return first + i * step;
			}
		}

		#endregion

		#region Public static methods

		#region Angle reduction

		/// <summary>
		/// Reduces unnecessary turns so that the angle is in the first turn. Returns the corrected value
		/// </summary>
		/// <param name="angle">Angle to reduce</param>
		/// <param name="unit">Unit the angle is denoted in, default is radians</param>
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
		/// Converts an angle value from one unit to another
		/// </summary>
		/// <param name="angle">Angle to reduce</param>
		/// <param name="angleUnit">Unit the angle is currently in</param>
		/// <param name="targetUnit">Unit to which convert the angle value to</param>
		/// <returns></returns>
		public static double ConvertAngle(double angle, AngleUnit angleUnit, AngleUnit targetUnit)
		{
			// If the target units are identical just return the value
			if(angleUnit == targetUnit)
			{
				return angle;
			}

			// Convert the angle to turns (if the angle is already in turns no conversion is needed)
			switch (angleUnit)
			{
				case AngleUnit.Radians:
					{
						angle /= 2 * Math.PI;
					}
					break;

				case AngleUnit.Degrees:
					{
						angle /= 360;
					}
					break;
			}

			// Convert the angle to target unit (if the target is turns then no additional conversion is required)
			switch (targetUnit)
			{
				case AngleUnit.Radians:
					{
						angle *= 2 * Math.PI;
					}
					break;

				case AngleUnit.Degrees:
					{
						angle *= 360;
					}
					break;
			}

			return angle;
		}

		#endregion

		#region Rounding to a multiple

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

		#region Rounding to digit

		/// <summary>
		/// Returns a double number rounded to the given digit
		/// </summary>
		/// <param name="value">Number to round</param>
		/// <param name="digit">Digit to round the number to</param>
		/// <param name="rounding"></param>
		/// <returns></returns>
		public static double RoundToDigit(this double value, int digit, MidpointRounding rounding = MidpointRounding.AwayFromZero)
		{
			// Get the order of the value (value = x * 10^order, 1 <= x < 10)
			var order = Math.Log10(Math.Abs(value));

			// Round it
			var roundedOrder =  Math.Floor(order);

			// Round the value after multiplying it by the given power of 10 (this action ensures that the digit to round to is
			// the one on the left of the decimal), if value is <1 then the order used should be 0
			var roundedValue = Math.Round(value * Math.Pow(10, digit - Math.Max(roundedOrder,0) - 1), rounding);

			// Finally divide the value by 10 to power which is a difference of the newly obtained order and the previous order,
			// this recovers the order from the beginning of the method (not necessary if the rounding resulted in 0)
			if (roundedValue != 0)
			{
				// Get the order of the new value
				var newOrder = Math.Log10(Math.Abs(roundedValue));


				// If the log is an integer it means that rounded value was a power of 10 and will result in a log greater by 1 than
				// actually needed (unless the original value was a power of 10 or difference between it and the neighbouring power
				// of 10 was smaller than 10^(-digit) which is represented by the second condition).				
				if (newOrder == Math.Floor(newOrder) && Math.Abs(value) - Math.Pow(10, roundedOrder) > Math.Pow(10, -digit))
				{
					--newOrder;
				}

				roundedValue /= Math.Pow(10, Math.Floor(newOrder) - roundedOrder);
			}

			return roundedValue;
		}

		/// <summary>
		/// Returns a double number rounded down to the given digit
		/// </summary>
		/// <param name="value">Number to round</param>
		/// <param name="digit">Digit to round the number to</param>
		/// <returns></returns>
		public static double FloorToDigit(this double value, int digit)
		{
			// Get the order of the value (value = x * 10^order, 1 <= x < 10)
			var order = Math.Floor(Math.Log10(Math.Abs(value)));

			// Round the value after multiplying it by the given power of 10 (this action ensures that the digit to round to is
			// the one on the left of the decimal), if value is <1 then the order used should be 0
			value = Math.Floor(value * Math.Pow(10, digit - Math.Max(order, 0) - 1));

			// Finally divide the value by 10 to power which is a difference of the newly obtained order and the previous order,
			// this recovers the order from the beginning of the method (not necessary if the rounding resulted in 0)
			if (value != 0)
			{
				value /= Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))) - order);
			}

			return value;
		}

		/// <summary>
		/// Returns a double number rounded up to the given digit
		/// </summary>
		/// <param name="value">Number to round</param>
		/// <param name="digit">Digit to round the number to</param>
		/// <returns></returns>
		public static double CeilingToDigit(this double value, int digit)
		{
			// Get the order of the value (value = x * 10^order, 1 <= x < 10)
			var order = Math.Floor(Math.Log10(Math.Abs(value)));

			// Round the value after multiplying it by the given power of 10 (this action ensures that the digit to round to is
			// the one on the left of the decimal), if value is <1 then the order used should be 0
			value = Math.Ceiling(value * Math.Pow(10, digit - Math.Max(order, 0) - 1));

			// Finally divide the value by 10 to power which is a difference of the newly obtained order and the previous order,
			// this recovers the order from the beginning of the method (not necessary if the rounding resulted in 0)
			if (value != 0)
			{
				value /= Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))) - order);
			}

			return value;
		}

		/// <summary>
		/// Returns a Complex number rounded to the given digit (both real and imaginary parts are rounded)
		/// </summary>
		/// <param name="value">Number to round</param>
		/// <param name="digit">Digit to round the number to</param>
		/// <param name="rounding"></param>
		/// <returns></returns>
		public static Complex RoundToDigit(this Complex value, int digit, MidpointRounding rounding = MidpointRounding.AwayFromZero) =>
			// Use the method created for doubles to round both real and imaginary parts
			new Complex(value.Real.RoundToDigit(digit, rounding), value.Imaginary.RoundToDigit(digit, rounding));

		/// <summary>
		/// Returns a Complex number rounded down to the given digit (both real and imaginary parts are rounded)
		/// </summary>
		/// <param name="value">Number to round</param>
		/// <param name="digit">Digit to round the number to</param>		
		/// <returns></returns>
		public static Complex FloorToDigit(this Complex value, int digit) =>
			// Use the method created for doubles to round both real and imaginary parts
			new Complex(value.Real.FloorToDigit(digit), value.Imaginary.FloorToDigit(digit));

		/// <summary>
		/// Returns a Complex number rounded up to the given digit (both real and imaginary parts are rounded)
		/// </summary>
		/// <param name="value">Number to round</param>
		/// <param name="digit">Digit to round the number to</param>		
		/// <returns></returns>
		public static Complex CeilingToDigit(this Complex value, int digit) =>
			// Use the method created for doubles to round both real and imaginary parts
			new Complex(value.Real.CeilingToDigit(digit), value.Imaginary.CeilingToDigit(digit));

		#endregion

		#region Casting

		/// <summary>
		/// Tries to cast value to each numeral type. If it succeeds returns it as double. Otherwise returns exception;
		/// </summary>
		/// <param name="value"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException"></exception>
		public static double CastToDouble(object value)
		{
			if(TryCastToDouble(value, out var result))
			{
				return result;
			}

			throw new InvalidCastException();
		}

		/// <summary>
		/// Tries to cast value to each numeral type. If it succeeds, assigns it to <paramref name="result"/> and returns true.
		/// Otherwise returns false;
		/// </summary>
		/// <param name="value"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public static bool TryCastToDouble(object value, out double result)
		{
			if (value is sbyte sb)
			{
				result = sb;
				return true;
			}

			if (value is byte b)
			{
				result = b;
				return true;
			}

			if (value is short s)
			{
				result = s;
				return true;
			}

			if (value is ushort us)
			{
				result = us;
				return true;
			}

			if (value is int i)
			{
				result = i;
				return true;
			}

			if (value is uint ui)
			{
				result = ui;
				return true;
			}

			if (value is long l)
			{
				result = l;
				return true;
			}

			if (value is ulong ul)
			{
				result = ul;
				return true;
			}

			if (value is float f)
			{
				result = f;
				return true;
			}

			if (value is double d)
			{
				result = d;
				return true;
			}

			result = 0;
			return false;
		}

		#endregion

		#region Data set related

		/// <summary>
		/// Returns <paramref name="pointsCount"/> number of evenly spaced points between <paramref name="first"/> and
		/// <paramref name="last"/> (first is <paramref name="first"/> and last is <paramref name="last"/>).
		/// For <paramref name="pointsCount"/> smaller than 0 throws an exception, for <paramref name="pointsCount"/> equal to 0
		/// returns empty enumeration, for <paramref name="pointsCount"/> equal to 1 returns the <paramref name="first"/> if it's
		/// equal to <paramref name="last"/>, otherwise throws an exception.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="last"></param>
		/// <param name="pointsCount"></param>
		/// <returns></returns>
		public static IEnumerable<double> CalculateMidPoints(double first, double last, int pointsCount)
		{
			// Number of points cannot be negative
			if(pointsCount < 0)
			{
				throw new ArgumentException(nameof(pointsCount) + " can't be smaller than 0");
			}

			// For no points return an empty enumeration
			if(pointsCount == 0)
			{
				return Enumerable.Empty<double>();
			}
						
			if(pointsCount == 1)
			{
				// If both points are identical return that value
				if(first == last)
				{
					return new double[] { first };
				}
				// Otherwise the result cannot be computed
				else
				{
					throw new ArgumentException(
						"If " + nameof(pointsCount) + " is 1 then " + nameof(first) + " has to be equal to " + nameof(last));
				}
			}

			// In case of 2 or more points, use the helper method
			return CalculateMidPointsHelper(first, last, pointsCount);
		}

		#endregion

		#endregion
	}
}