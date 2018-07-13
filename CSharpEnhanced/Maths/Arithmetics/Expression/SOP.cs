using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// A sum of products - contains one or more <see cref="IExpression"/>s that together form a sum
	/// </summary>
	public class SOP : IExpression
    {
		#region Constructors

		/// <summary>
		/// Default Constructor
		/// </summary>
		public SOP()
		{
			_Summands = new List<IExpression>();
			Summands = new ReadOnlyCollection<IExpression>(_Summands);
		}

		/// <summary>
		/// Creates a new <see cref="SOP"/> based on the parameter (summands and sign are copied)
		/// </summary>
		/// <param name="sop"></param>
		public SOP(SOP sop)
		{
			// Copy its summands and sign
			_Summands = new List<IExpression>(sop.Summands);
			Sign = sop.Sign;

			Summands = new ReadOnlyCollection<IExpression>(_Summands);
		}

		/// <summary>
		/// Constructs an <see cref="SOP"/> with one summand. To maximize performance (and minimize nested sums)
		/// the <paramref name="expression"/> should not be an <see cref="SOP"/>
		/// </summary>
		/// <param name="expression"></param>
		public SOP(IExpression expression)
		{				
			_Summands = new List<IExpression>() { expression };
			Summands = new ReadOnlyCollection<IExpression>(_Summands);
		}

		/// <summary>
		/// Constructs an <see cref="SOP"/> with two summands. To maximize performance (and minimize nested sums)
		/// the <paramref name="ex1"/> and <paramref name="ex2"/> should not be <see cref="SOP"/>s
		/// </summary>
		/// <param name="expression"></param>
		public SOP(IExpression ex1, IExpression ex2)
		{
			_Summands = new List<IExpression>() { ex1, ex2 };
			Summands = new ReadOnlyCollection<IExpression>(_Summands);
		}

		/// <summary>
		/// Constructs an <see cref="SOP"/> based on the given summands
		/// </summary>
		/// <param name="summands"></param>
		private SOP(IEnumerable<IExpression> summands)
		{
			_Summands = new List<IExpression>(summands);
			Summands = new ReadOnlyCollection<IExpression>(_Summands);
		}

		/// <summary>
		/// Constructs an <see cref="SOP"/> based on the given summands and sign
		/// </summary>
		/// <param name="summands"></param>
		/// <param name="sign"></param>
		private SOP(IEnumerable<IExpression> summands, bool sign) : this(summands)
		{
			Sign = sign;
		}

		/// <summary>
		/// Constructs an <see cref="SOP"/> based on the given summands
		/// </summary>
		/// <param name="summands"></param>
		private SOP(List<IExpression> summands)
		{
			_Summands = summands;
			Summands = new ReadOnlyCollection<IExpression>(_Summands);
		}

		/// <summary>
		/// Constructs an <see cref="SOP"/> based on the given summands and sign
		/// </summary>
		/// <param name="summands"></param>
		/// <param name="sign"></param>
		private SOP(List<IExpression> summands, bool sign) : this(summands)
		{
			Sign = sign;
		}

		#endregion

		#region Private properties

		/// <summary>
		/// Backign store for <see cref="Summands"/>
		/// </summary>
		private List<IExpression> _Summands { get; } = new List<IExpression>();

		#endregion

		#region Public properties

		/// <summary>
		/// Collection of all summands that make up this <see cref="SOP"/>
		/// </summary>
		public ReadOnlyCollection<IExpression> Summands { get; }

		/// <summary>
		/// Sign in front of the expression - true indicates a minus, false indicates a plus (no impact on the value)
		/// </summary>
		public bool Sign { get; }

		#endregion

		#region Public methods

		/// <summary>
		/// Returns the value of this <see cref="IExpression"/>
		/// </summary>
		/// <returns></returns>
		public Complex Evaluate()
		{
			return Complex.Zero;
		}

		/// <summary>
		/// Returns a negation of the <see cref="SOP"/>
		/// </summary>
		/// <returns></returns>
		public IExpression Negation() => new SOP(Summands, !Sign);

		/// <summary>
		/// Returns a reciprocal of the <see cref="SOP"/> wrapped in a <see cref="POS"/>
		/// </summary>
		/// <returns></returns>
		public IExpression Reciprocal() => new POS(this).Reciprocal();

		/// <summary>
		/// Adds the <paramref name="expression"/> to this <see cref="SOP"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Add(IExpression expression)
		{
			// If the other expression is an SOP
			if(expression is SOP sum)
			{
				// Check if the signs match
				return new SOP(Sign == sum.Sign ?
					// If so, just concat lists of summands
					Summands.Concat(sum.Summands) :
					// Otherwise negate the second expression
					Summands.Concat(sum.Summands.Select((x) => x.Negation())), Sign);
			}
			// If it's not simply create a new SOP based on it and add it to the current instance
			else
			{
				return Add(new SOP(expression));
			}
		}

		/// <summary>
		/// Subtracts <paramref name="expression"/> from this <see cref="SOP"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Subtract(IExpression expression) => Add(expression.Negation());

		/// <summary>
		/// Mutliplies this <see cref="SOP"/> by <paramref name="expression"/>, result is wrapped in a <see cref="POS"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Multiply(IExpression expression)
		{
			// If the other expression is a POS
			if(expression is POS product)
			{
				// Multiply it by this
				return product.Multiply(this);
			}
			else
			{
				// Otherwise return a new POS that is a product of this and expression
				return new POS(this, expression);
			}
		}

		/// <summary>
		/// Divides this <see cref="SOP"/> by <paramref name="expression"/>, result is a <see cref="POS"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Divide(IExpression expression) => new POS(this, expression.Reciprocal());

		#endregion
	}
}