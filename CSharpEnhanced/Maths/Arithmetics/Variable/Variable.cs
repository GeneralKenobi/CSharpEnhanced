using CSharpEnhanced.CoreClasses;
using System.Collections.Generic;
using System.Numerics;

namespace CSharpEnhanced.Maths
{
	/// <summary>
	/// <see cref="Variable"/> is a class supporting the idea of symbolical solving of some math problems. When utilizing it, one
	/// may compute a general solution, for eg. X = Var1 + Var2*Var3 where it's no longer necessary to manipulate the system, only
	/// a simple substitution of values for Vars will yield a result.
	/// A good exampele of usage would be when simulating an electronic circuit. An admittance matrix would be constructed and,
	/// normally, a system of linear equations would have to be solved whenever some parameter changes (for example for AC analysis
	/// the voltge changes with time). Instead one may compute a general solution using <see cref="Variable"/>s and then simply
	/// substitute admittances, sources and so on to obtain valid solution. The initial solution of the system will be more
	/// time-consuming but it will significantly reduce all solutions for systems of the same structure.
	/// Variables may not be created directly, instead they can be obtained from a <see cref="VariableSource"/>.
	/// <see cref="Variable"/>s may only read the value, <see cref="VariableSource"/> may read it as well as set it.
	/// </summary>
	public partial class Variable : IExpression
    {
		#region Constructor

		/// <summary>
		/// Constructor with parameters for static constants and implicit conversions
		/// </summary>
		private Variable(Complex value)
		{
			_Value = new RefWrapper<Complex>(value);
		}

		/// <summary>
		/// Constructor with parameters for construction with <see cref="VariableSource"/>
		/// </summary>
		/// <param name="value"></param>
		private Variable(RefWrapper<Complex> value, RefWrapper<string> label)
		{
			_Value = value;
			_Label = label;
		}

		#endregion

		#region Private properties

		/// <summary>
		/// Reference to the label set in the parent <see cref="VariableSource"/>
		/// </summary>
		private RefWrapper<string> _Label { get; }

		/// <summary>
		/// Reference to the value set in the parent <see cref="VariableSource"/>
		/// </summary>
		private RefWrapper<Complex> _Value { get; }

		#endregion

		#region Public properties

		/// <summary>
		/// Label assigned to this <see cref="Variable"/>
		/// </summary>
		public string Label => _Label.Value;

		/// <summary>
		/// Value of this <see cref="Variable"/>
		/// </summary>
		public Complex Value => new Complex(_Value.Value.Real, _Value.Value.Imaginary);

		/// <summary>
		/// True if the imaginary component is equal to 0
		/// </summary>
		public bool IsPureReal => Value.Imaginary == 0;

		/// <summary>
		/// True if the real component is equal to 0
		/// </summary>
		public bool IsPureImaginary => Value.Real == 0;

		#endregion

		#region Public static properties

		/// <summary>
		/// Pure real one
		/// </summary>
		public static Variable One { get; } = new Variable(Complex.One);
		
		/// <summary>
		/// Pure real negative one
		/// </summary>
		public static Variable NegativeOne { get; } = new Variable(-Complex.One);

		/// <summary>
		/// Zero
		/// </summary>
		public static Variable Zero { get; } = new Variable(Complex.Zero);

		/// <summary>
		/// Pure imaginary one
		/// </summary>
		public static Variable ImaginaryOne { get; } = new Variable(Complex.ImaginaryOne);

		/// <summary>
		/// Pure imaginary negative one
		/// </summary>
		public static Variable NegativeImaginaryOne { get; } = new Variable(-Complex.ImaginaryOne);

		#endregion

		#region Implicit conversion

		/// <summary>
		/// Implicit conversion of a <see cref="Complex"/> into a <see cref="Variable"/>
		/// </summary>
		/// <param name="c"></param>
		public static implicit operator Variable(Complex c) => new Variable(c);		

		#endregion

		#region Public methods

		/// <summary>
		/// Returns the value of this variable
		/// </summary>
		/// <returns></returns>
		public Complex Evaluate() => Value;

		/// <summary>
		/// Returns a negation of this <see cref="Variable"/> wrapped in an <see cref="SOP"/>
		/// </summary>
		/// <returns></returns>
		public IExpression Negation() => new SOP(this).Negation();

		/// <summary>
		/// Returns the reciprocal of this <see cref="Variable"/> wrapped in an <see cref="POS"/>
		/// </summary>
		/// <returns></returns>
		public IExpression Reciprocal() => new POS(this).Reciprocal();

		/// <summary>
		/// Adds this <see cref="Variable"/> to the <paramref name="expression"/>, the result is wrapped in an <see cref="SOP"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Add(IExpression expression) => new SOP(this).Add(expression);

		/// <summary>
		/// Adds <paramref name="expression"/> from this <see cref="Variable"/>, the result is wrapped in an <see cref="SOP"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Subtract(IExpression expression) => new SOP(this).Subtract(expression);

		/// <summary>
		/// Mutliplies this <see cref="Variable"/> by <paramref name="expression"/>, result is wrapped in a <see cref="POS"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Multiply(IExpression expression) => new POS(this).Multiply(expression);

		/// <summary>
		/// Divides this <see cref="Variable"/> by <paramref name="expression"/>, result is wrapped in a <see cref="POS"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IExpression Divide(IExpression expression) => new POS(this).Divide(expression);

		/// <summary>
		/// Overrides the ToString method, returns either "*Label* = *Value*" if label is not null or simply "*Value*"
		/// </summary>
		/// <returns></returns>
		public override string ToString() => Label ?? Value.ToString();

		#endregion
	}
}