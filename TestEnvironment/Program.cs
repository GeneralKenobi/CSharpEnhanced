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


			var system = new SystemOfLinearEquations();

			system.Coefficients = new IExpression[,] { { x, y, z}, { y, z, w}, { z, w, x } };
			system.FreeTerms = new IExpression[] { w, x, y};
			//system.Coefficients = new IExpression[,] { { x, w }, { w, y } };
			//system.FreeTerms = new IExpression[] { z, z };
			Stopwatch s = new Stopwatch();
			s.Start();
			system.Solve();
			s.Stop();
			//Console.WriteLine(s.ElapsedMilliseconds);

			Console.WriteLine(system.FreeTerms[0].Evaluate());
			Console.WriteLine(system.FreeTerms[1].Evaluate());
			Console.WriteLine(system.FreeTerms[2].Evaluate());
			
			for(int i=0; i<system.Rows; ++i)
			{
				for(int j=0; j<system.Columns; ++j)
				{
					Console.Write(system.Coefficients[i, j].Evaluate().ToString() + "\t");
				}

				Console.WriteLine();
			}

			Console.ReadLine();
		}
	}
}
