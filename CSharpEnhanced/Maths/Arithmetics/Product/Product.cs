using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// Linear monomial is a monomial of degree at most 1, eg. 5 or 3+5i or (1+2i)x
	/// </summary>
	public class Product
    {
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public Product() { }

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		public Product(List<Variable> numerator, List<Variable> denominator)
		{
			Numerator = numerator;
			Denominator = denominator;
		}

		#endregion

		#region Public Properties		

		/// <summary>
		/// Numerator is a product of all entries in this list
		/// </summary>
		public List<Variable> Numerator { get; } = new List<Variable>();

		/// <summary>
		/// Denominator is a product of all entries in this list
		/// </summary>
		public List<Variable> Denominator { get; } = new List<Variable>();

		#endregion

		#region Operators

		/// <summary>
		/// Multiplies numerators and denominators, then reduces the product
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		public static Product operator*(Product p1, Product p2)
		{
			var result = new Product(new List<Variable>(p1.Numerator.Concat(p2.Numerator)),
				new List<Variable>(p1.Denominator.Concat(p2.Denominator)));

			result.Reduce();

			return result;
		}

		/// <summary>
		/// Multiplies numerator with denominator and denominator with numerator, then reduces the product
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		public static Product operator /(Product p1, Product p2)
		{
			var result = new Product(new List<Variable>(p1.Numerator.Concat(p2.Denominator)),
				new List<Variable>(p1.Denominator.Concat(p2.Numerator)));

			result.Reduce();

			return result;
		}

		/// <summary>
		/// Returns a sum of two products (an expression)
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		public static Expression operator +(Product p1, Product p2)
		{
			return new Expression()
			{
				Products = new List<Product>()
				{
					p1, p2
				}
			};
		}

		/// <summary>
		/// Returns a difference of two products (an expression)
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		public static Expression operator -(Product p1, Product p2)
		{
			return new Expression()
			{
				Products = new List<Product>()
				{
					p1, -p2
				}
			};
		}

		/// <summary>
		/// Returns a negation of <paramref name="p"/>
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static Product operator -(Product p)
		{
			var result = new Product(new List<Variable>(p.Numerator), new List<Variable>(p.Denominator));
			
			if(result.Numerator.Contains(Variable.NegativeOne))
			{
				result.Numerator.Remove(Variable.NegativeOne);
			}
			else
			{
				result.Numerator.Add(Variable.NegativeOne);
			}

			return result;
		}

		#endregion

		#region Implicit conversion

		/// <summary>
		/// Converts the <see cref="Product"/> into an <see cref="Expression"/> with <paramref name="p"/> in its products list
		/// </summary>
		/// <param name="p"></param>
		public static implicit operator Expression(Product p) => new Expression() { Products = new List<Product>() { p } };

		#endregion

		#region Private methods

		/// <summary>
		/// Reduces the product - if a term apears in both numerator and denominator removes it
		/// </summary>
		private void Reduce()
		{
			// Loop over every entry in denominator
			for(int i = 0; i<Denominator.Count; ++i)
			{
				// If the entry is present in the numerator
				if(Numerator.Contains(Denominator[i]))
				{
					// Remove it from the numerator and denominator
					Numerator.Remove(Denominator[i]);
					Denominator.RemoveAt(i);

					// Decrement i because the element at i-th index has changed
					--i;
				}
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Returns the value of the product
		/// </summary>
		/// <returns></returns>
		public Complex Evaluate()
		{
			var result = Complex.Zero;

			Numerator.ForEach((x) => result *= x.Value);
			Denominator.ForEach((x) => result /= x.Value);

			return result;
		}

		/// <summary>
		/// Returns a new product that is a reciprocal of the instance
		/// </summary>
		/// <returns></returns>
		public Product Reciprocal() => new Product(new List<Variable>(Denominator), new List<Variable>(Numerator));

		#endregion
	}
}