using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// An expression consisting of <see cref="Product"/>
	/// </summary>
    public class Expression
    {
		#region Public properties

		/// <summary>
		/// Monomials that make up this <see cref="Expression"/> as a sum (i.e. M1 + M2 + ... + Mn)
		/// </summary>
		public List<Product> Monomials { get; set; }

		#endregion

		#region Public methods

		/// <summary>
		/// Returns the value of the expression
		/// </summary>
		/// <returns></returns>
		public Complex Evaluate()
		{
			var result = Complex.Zero;

			Monomials.ForEach((x) => result += x.Coefficient * x.Var.Value);

			return result;
		}

		#endregion

		#region Operators

		/// <summary>
		/// Returns a sum of two <see cref="Expression"/>s
		/// </summary>
		/// <param name="ex1"></param>
		/// <param name="ex2"></param>
		/// <returns></returns>
		public static Expression operator +(Expression ex1, Expression ex2) => new Expression()
		{
			Monomials = new List<Product>(ex1.Monomials.Concat(ex2.Monomials)),
		};

		/// <summary>
		/// Returns a difference of two <see cref="Expression"/>s
		/// </summary>
		/// <param name="ex1"></param>
		/// <param name="ex2"></param>
		/// <returns></returns>
		public static Expression operator -(Expression ex1, Expression ex2) => new Expression()
		{
			Monomials = new List<Product>(ex1.Monomials.Concat(ex2.Monomials.Select(
				(x) => new Product() { Coefficient = -x.Coefficient, Var = x.Var }))),
		};

		public static Expression operator *(Expression ex1, Expression ex2)
		{
			var result = new Expression();

			ex1.Monomials.ForEach((ex1Monomial) => ex2.Monomials.ForEach((ex2Monomial) =>
			{
				result.Monomials.Add(ex1Monomial * ex2Monomial);
			}));


			return null;
		}

		/// <summary>
		/// Multiplies every coefficient in <see cref="Monomials"/> by <paramref name="c"/>
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public static Expression operator *(Expression ex, Complex c) => new Expression()
		{
			Monomials = new List<Product>(ex.Monomials.Select((x) => new Product()
			{
				Var = x.Var,
				Coefficient = x.Coefficient * c,
			})),
		};

		/// <summary>
		/// Multiplies every coefficient in <see cref="Monomials"/> by <paramref name="c"/>
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public static Expression operator *(Complex c, Expression ex) => ex * c;

		/// <summary>
		/// Divides every coefficient in <see cref="Monomials"/> by <paramref name="c"/>
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public static Expression operator /(Expression ex, Complex c) => ex * Complex.Reciprocal(c);

		/// <summary>
		/// Divides every coefficient in <see cref="Monomials"/> by <paramref name="c"/>
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public static Expression operator /(Complex c, Expression ex) => ex / c;

		/// <summary>
		/// Returns a <see cref="Expression"/> with <paramref name="m"/> added to it
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		public static Expression operator +(Expression ex, Product m) => new Expression()
		{
			Monomials = new List<Product>(ex.Monomials.Concat(new List<Product>() { m, })),			
		};

		/// <summary>
		/// Returns a <see cref="Expression"/> with <paramref name="m"/> added to it
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		public static Expression operator +(Product m, Expression ex) => ex + m;

		/// <summary>
		/// Returns a <see cref="Expression"/> with <paramref name="m"/> subtracted from it
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		public static Expression operator -(Expression ex, Product m) => ex + new Product()
		{
			Var = m.Var,
			Coefficient = -m.Coefficient,
		};

		/// <summary>
		/// Returns a <see cref="Expression"/> with <paramref name="m"/> subtracted from it
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		public static Expression operator -(Product m, Expression ex) => ex - m;

		#endregion
	}
}