using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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
		public List<Product> Products { get; set; } = new List<Product>();

		#endregion

		#region Public methods

		/// <summary>
		/// Returns the value of the expression
		/// </summary>
		/// <returns></returns>
		public Complex Evaluate()
		{
			var result = Complex.Zero;

			Products.ForEach((x) => result += x.Evaluate());

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
			Products = new List<Product>(ex1.Products.Concat(ex2.Products)),
		};

		/// <summary>
		/// Returns a difference of two <see cref="Expression"/>s
		/// </summary>
		/// <param name="ex1"></param>
		/// <param name="ex2"></param>
		/// <returns></returns>
		public static Expression operator -(Expression ex1, Expression ex2) => new Expression()
		{
			Products = new List<Product>(ex1.Products.Concat(ex2.Products.Select((x) => -x))),
		};

		/// <summary>
		/// Returns a product of two expressions (multiplies every product in <paramref name="ex1"/> with every product in
		/// <paramref name="ex2"/>
		/// </summary>
		/// <param name="ex1"></param>
		/// <param name="ex2"></param>
		/// <returns></returns>
		public static Expression operator *(Expression ex1, Expression ex2)
		{
			var result = new Expression();

			ex1.Products.ForEach((ex1Monomial) => ex2.Products.ForEach((ex2Monomial) =>
			{
				result.Products.Add(ex1Monomial * ex2Monomial);
			}));

			return result;
		}		

		/// <summary>
		/// Returns a <see cref="Expression"/> with <paramref name="m"/> added to it
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		public static Expression operator +(Expression ex, Product m) => new Expression()
		{
			Products = new List<Product>(ex.Products.Concat(new List<Product>() { m })),
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
		public static Expression operator -(Expression ex, Product m) => ex + (-m);

		/// <summary>
		/// Returns a <see cref="Expression"/> with <paramref name="m"/> subtracted from it
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		public static Expression operator -(Product m, Expression ex) => new Expression()
		{
			Products = new List<Product>(ex.Products.Select((x) => -x).Concat(new List<Product>() { m }))
		};		

		#endregion
	}
}