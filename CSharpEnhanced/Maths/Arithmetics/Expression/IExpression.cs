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

		/// <summary>
		/// Adds two expressions
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		IExpression Add(IExpression expression);

		/// <summary>
		/// Subtracts <paramref name="expression"/> from this <see cref="IExpression"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		IExpression Subtract(IExpression expression);

		/// <summary>
		/// Mutliplies two <see cref="IExpression"/>s
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		IExpression Multiply(IExpression expression);

		/// <summary>
		/// Divides this <see cref="IExpression"/> by <paramref name="expression"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		IExpression Divide(IExpression expression);

		#endregion
	}
}