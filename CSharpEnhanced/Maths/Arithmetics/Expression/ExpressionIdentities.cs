using System.Numerics;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Identities that may be used to manipulate <see cref="IExpression"/>s
	/// </summary>
	public class ExpressionIdentities : IExpression
	{
		#region Constructor

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		private ExpressionIdentities(Complex value)
		{
			Value = value;
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		private ExpressionIdentities(double realComponent)
		{
			Value = new Complex(realComponent, 0);
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Returns the value of the <see cref="IExpression"/>
		/// </summary>
		/// <returns></returns>
		public Complex Evaluate() => Value;

		/// <summary>
		/// Returns a negation of the <see cref="IExpression"/>
		/// </summary>
		/// <returns></returns>
		public IExpression Negate() => new ExpressionIdentities(-Value);

		#endregion

		#region Public properties

		/// <summary>
		/// The value of this instance
		/// </summary>
		public Complex Value { get; }

		#endregion

		#region Public static properties

		/// <summary>
		/// Product identity - real one
		/// </summary>
		public static ExpressionIdentities One { get; } = new ExpressionIdentities(1);

		/// <summary>
		/// Negative one
		/// </summary>
		public static ExpressionIdentities NegativeOne { get; } = new ExpressionIdentities(-1);

		/// <summary>
		/// Sum identity - zero
		/// </summary>
		public static ExpressionIdentities Zero { get; } = new ExpressionIdentities(0);

		#endregion
	}
}