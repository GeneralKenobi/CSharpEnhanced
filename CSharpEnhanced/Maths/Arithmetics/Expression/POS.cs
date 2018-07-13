using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// A product of sums - contains one or more <see cref="IExpression"/>s that together form a product.
	/// Important: empty product is by convention equal to 1
	/// </summary>
	public class POS : IExpression
    {
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public POS()
		{
			_Factors = new List<IExpression>();
			Factors = new ReadOnlyCollection<IExpression>(_Factors);
		}

		/// <summary>
		/// Creates a new <see cref="POS"/> based on the parameter (summands and sign are copied)
		/// </summary>
		/// <param name="pos"></param>
		public POS(POS pos)
		{
			// Copy its summands and sign
			_Factors = new List<IExpression>(pos.Factors);
			Power = pos.Power;

			Factors = new ReadOnlyCollection<IExpression>(_Factors);
		}

		/// <summary>
		/// Constructs a <see cref="POS"/> with one factor
		/// </summary>
		/// <param name="expression"></param>
		public POS(IExpression expression)
		{
			_Factors = new List<IExpression>() { expression };
			Factors = new ReadOnlyCollection<IExpression>(_Factors);
		}

		/// <summary>
		/// Constructs a <see cref="POS"/> with two factors
		/// </summary>
		/// <param name="ex1"></param>
		/// <param name="ex2"></param>
		public POS(IExpression ex1, IExpression ex2)
		{
			_Factors = new List<IExpression>() { ex1, ex2 };
			Factors = new ReadOnlyCollection<IExpression>(_Factors);
		}

		/// <summary>
		/// Constructs a <see cref="POS"/> with the given factors
		/// </summary>
		/// <param name="factors"></param>
		private POS(IEnumerable<IExpression> factors)
		{
			_Factors = new List<IExpression>(factors);
			Factors = new ReadOnlyCollection<IExpression>(_Factors);
		}

		/// <summary>
		/// Constructs a <see cref="POS"/> with the given factors and power
		/// </summary>
		/// <param name="factors"></param>
		private POS(IEnumerable<IExpression> factors, bool power) : this(factors)
		{
			Power = power;
		}

		/// <summary>
		/// Constructs a <see cref="POS"/> with the given factors
		/// </summary>
		/// <param name="factors"></param>
		private POS(List<IExpression> factors)
		{
			_Factors = factors;
			Factors = new ReadOnlyCollection<IExpression>(_Factors);
		}

		/// <summary>
		/// Constructs a <see cref="POS"/> with the given factors and power
		/// </summary>
		/// <param name="factors"></param>
		private POS(List<IExpression> factors, bool power) : this(factors)
		{
			Power = power;
		}

		#endregion

		#region Private properties

		/// <summary>
		/// Backing store for <see cref="Factors"/>
		/// </summary>
		private List<IExpression> _Factors { get; } = new List<IExpression>();

		#endregion

		#region Public properties

		/// <summary>
		/// Collection of all factors that make up this <see cref="POS"/>
		/// </summary>
		public ReadOnlyCollection<IExpression> Factors { get; }

		/// <summary>
		/// Power to which the <see cref="IExpression"/> is raised (a simplified solution - true for -1 (reciprocal),
		/// false for 1 (no impact on the value))
		/// </summary>
		public bool Power { get; }

		#endregion

		#region Public methods

		/// <summary>
		/// Returns the value of this <see cref="POS"/>. Important: empty product is by convention equal to 1
		/// </summary>
		/// <returns></returns>
		public Complex Evaluate()
		{
			var result = Complex.One;

			// Multiply together all factors
			_Factors.ForEach((x) => result *= x.Evaluate());

			// If the power is -1 (true) then return a reciprocal
			return Power ? Complex.Reciprocal(result) : result;
		}

		/// <summary>
		/// Returns a negation of this <see cref="POS"/> wrapped in an <see cref="SOP"/>
		/// </summary>
		/// <returns></returns>
		public IExpression Negation() => new SOP(this).Negation();

		/// <summary>
		/// Returns a reciprocal of the <see cref="POS"/>
		/// </summary>
		/// <returns></returns>
		public IExpression Reciprocal() => new POS(Factors, !Power);

		/// <summary>
		/// Adds this <see cref="POS"/> to the <see cref="IExpression"/>, result is wrapped in an <see cref="SOP"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Add(IExpression expression)
		{
			// If the expression is a sum
			if(expression is SOP sum)
			{
				// Add this to the sum
				return sum.Add(this);
			}
			else
			{
				// Otherwise create a new SOP and return it
				return new SOP(this, expression);
			}
		}

		/// <summary>
		/// Subtracts <paramref name="expression"/> from this <see cref="POS"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Subtract(IExpression expression)
		{
			// If the expression is a sum
			if(expression is SOP sum)
			{
				// Subtract sum from this [(this - expression)] and flip signs [-(this - expression)] = [expression - this]
				return sum.Subtract(this).Negation();
			}
			else
			{
				// Otherwise create a new SOP and return it
				return new SOP(this, expression.Negation());
			}
		}

		/// <summary>
		/// Multiplies this <see cref="POS"/> by <paramref name="expression"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Multiply(IExpression expression)
		{
			// If the other expression as a POS
			if(expression is POS product)
			{
				// Check if the powers match
				return new POS(Power == product.Power?
					// If so, just concat lists of factors
					Factors.Concat(product.Factors) :
					// Otherwise invert the second expression
					Factors.Concat(product.Factors.Select((x) => x.Reciprocal())), Power);
			}
			// If it's not simply create a new POS based on it and multiply it by the current instance
			else
			{
				return Multiply(new POS(expression));
			}
		}

		/// <summary>
		/// Divides this <see cref="POS"/> by <paramref name="expression"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Divide(IExpression expression) => Multiply(expression.Reciprocal());

		/// <summary>
		/// Returns a string version of the expression
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if(Factors.Count == 0)
			{
				return string.Empty;
			}

			var result = "(";

			_Factors.ForEach((x) => result += x.ToString() + " * ");

			result = result.Substring(0, result.Length - 3);

			result += ")";

			if(Power)
			{
				result = result.Insert(0, "[");
				result += "^(-1)]";
			}

			return result;
		}

		#endregion
	}
}