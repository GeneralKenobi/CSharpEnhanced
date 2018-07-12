using System.Numerics;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Interface for algebraic expressions
	/// </summary>
	public interface IExpression
    {
		#region Methods

		/// <summary>
		/// Returns the value of the <see cref="IExpression"/>
		/// </summary>
		/// <returns></returns>
		Complex Evaluate();

		/// <summary>
		/// Returns a negation of the <see cref="IExpression"/>
		/// </summary>
		/// <returns></returns>
		IExpression Negation();

		/// <summary>
		/// Returns the reciprocal of the <see cref="IExpression"/>
		/// </summary>
		/// <returns></returns>
		IExpression Reciprocal();

		#endregion
	}
}