﻿using CSharpEnhanced.CoreClasses;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

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
	public partial class Variable
	{
		/// <summary>
		/// Used to construct Variables and set their value
		/// </summary>
		public class VariableSource
		{
			#region Constructor

			/// <summary>
			/// Default Constructor
			/// </summary>
			public VariableSource()
			{
				Variable = new Variable(ValueWrapper, LabelWrapper);
			}

			/// <summary>
			/// Constructor taking initial value as parameter
			/// </summary>
			/// <param name="initialValue"></param>
			public VariableSource(Complex initialValue) : this()
			{
				Value = initialValue;
			}

			/// <summary>
			/// Constructor taking initial real value as parameter
			/// </summary>
			/// <param name="initialValue"></param>
			public VariableSource(double initialRealValue) : this(new Complex(initialRealValue, 0)) { }

			/// <summary>
			/// Constructor taking real real value and imaginary initial value as parameters
			/// </summary>
			/// <param name="initialValue"></param>
			public VariableSource(double initialRealValue, double initialImaginaryValue)
				: this(new Complex(initialRealValue, initialImaginaryValue)) { }

			/// <summary>
			/// Constructor taking a label parameter
			/// </summary>
			/// <param name="label"></param>
			public VariableSource(string label) : this()
			{
				Label = label;
			}

			/// <summary>
			/// Constructor taking a label parameter and initial value as parameters
			/// </summary>
			/// <param name="label"></param>
			public VariableSource(Complex initialValue, string label) : this()
			{
				Value = initialValue;
				Label = label;
			}

			/// <summary>
			/// Constructor taking a label parameter and real initial value as parameters
			/// </summary>
			/// <param name="label"></param>
			public VariableSource(double initialRealValue, string label) : this(new Complex(initialRealValue, 0), label) { }

			/// <summary>
			/// Constructor taking a label parameter, real initial value and imaginary initial valuie as parameters
			/// </summary>
			/// <param name="label"></param>
			public VariableSource(double initialRealValue, double initialImaginaryValue, string label)
				: this(new Complex(initialRealValue, initialImaginaryValue), label) { }

			#endregion

			#region Public properties

			/// <summary>
			/// Accessors to the <see cref="ValueWrapper"/>.Value property
			/// </summary>
			public string Label
			{
				get => LabelWrapper.Value;
				set => LabelWrapper.Value = value;
			}

			/// <summary>
			/// Accessors to <see cref="ValueWrapper"/>'s Value property
			/// </summary>
			public Complex Value
			{
				get => ValueWrapper.Value;
				set => ValueWrapper.Value = value;
			}

			/// <summary>
			/// The value of the variable
			/// </summary>
			public RefWrapper<Complex> ValueWrapper { get; } = new RefWrapper<Complex>();

			/// <summary>
			/// The label assigned to this <see cref="VariableSource"/>
			/// </summary>
			public RefWrapper<string> LabelWrapper { get; } = new RefWrapper<string>();

			/// <summary>
			/// Variable that allows to read but not modify the value of this <see cref="VariableSource"/>
			/// </summary>
			public Variable Variable { get; }

			#endregion
		}
	}
}