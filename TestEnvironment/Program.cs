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


			var Coefficients = new IExpression[,] { { x, y, z}, { y, z, w}, { z, w, x } };
			var FreeTerms = new IExpression[] { w, x, y};
			//system.Coefficients = new IExpression[,] { { x, w }, { w, y } };
			//system.FreeTerms = new IExpression[] { z, z };
			Stopwatch s = new Stopwatch();
			s.Start();
			var solution = LinearEquations.SimplifiedGaussJordanElimination(Coefficients, FreeTerms);
			s.Stop();
			Console.WriteLine(s.ElapsedMilliseconds);

			Console.WriteLine(solution[0].Evaluate());
			Console.WriteLine(solution[1].Evaluate());
			Console.WriteLine(solution[2].Evaluate());
			
			Console.ReadLine();
		}
	}
}
