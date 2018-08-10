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
			var result = LinearEquations.SimplifiedGaussJordanElimination(
				new Complex[,] { { 1, 2, 0, 1, 0, 1 }, { 1, 1, 0, 0, 0, 1 }, { 0, 0, 0, 0, 0, 0 }, { 1, 0, 0, 0, 0, 2 }, { 0, 0, 0, 0, 0, 0 }, { 2, 1, 0, 0, 0, 0 } },
				new Complex[] { 0, 0, 0, 5 , 0, 4}, true); 


			string s = "5uV";

			var ss = SIHelpers.ParseSIString(s);
			
			Complex c = new Complex(0, 0);
			Console.WriteLine(SIHelpers.ToAltSIStringExcludingSmallPrefixes(c, "V"));
			c = new Complex(-21000, 220);
			Console.WriteLine(SIHelpers.ToAltSIStringExcludingSmallPrefixes(c, "Volts", true, true));
			c = new Complex(-21000, 0);
			Console.WriteLine(SIHelpers.ToAltSIStringExcludingSmallPrefixes(c, "Volts", true, true));
			//double d = 0.010000001;
			//double d1 = 10.00002000004;
			//double d2 = 9.99997000009;
			//double d3 = 0.00999997000009;
			//double d4 = 100;
			//double d5 = 0.1;
			//
			//Console.WriteLine(MathsHelpers.RoundToDigit(d, 4));
			//Console.WriteLine(MathsHelpers.RoundToDigit(d1, 4));
			//Console.WriteLine(MathsHelpers.RoundToDigit(d2, 4));
			//Console.WriteLine(MathsHelpers.RoundToDigit(d3, 4));
			//Console.WriteLine(MathsHelpers.RoundToDigit(d4, 4));
			//Console.WriteLine(MathsHelpers.RoundToDigit(d5, 4));



			Console.ReadLine();
		}
	}
}
