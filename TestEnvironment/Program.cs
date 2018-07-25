using CSharpEnhanced.Helpers;
using CSharpEnhanced.Maths;
using CSharpEnhanced.Synchronization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace TestEnvironment
{
	class Program
    {
		static void Main(string[] args)
		{
			var sx = new Variable.VariableSource(2, -3, "x");
			var sy = new Variable.VariableSource(5, 2, "y");
			var sz = new Variable.VariableSource(10, "z");
			var sw = new Variable.VariableSource(4, "w");

			var x = sx.Variable;
			var y = sy.Variable;
			var z = sz.Variable;
			var w = sw.Variable;

			//IExpression exp = x.Add(y).Multiply(z).Subtract(x.Multiply(y)).Divide(z.Multiply(y)).Divide(x.Add(y));

			//Console.Write(exp.ToString());


			//Console.Write($"\t = {exp.Evaluate()}");

		
			
			Console.ReadLine();
		}
	}
}
