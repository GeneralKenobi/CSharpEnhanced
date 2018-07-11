using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

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
		/// Coefficient of the monomial, by default 1
		/// </summary>
		public Complex Coefficient { get; set; } = Complex.One;

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
	}
}