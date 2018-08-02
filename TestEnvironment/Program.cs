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
			double d1 = 9.99997000009;
			double d2 = 0.00999997000009;
			double d3 = 100;
			double d4 = 0.1;

			Console.WriteLine(MathsHelpers.RoundToDigit(d1, 4));
			Console.WriteLine(MathsHelpers.RoundToDigit(d2, 4));
			Console.WriteLine(MathsHelpers.RoundToDigit(d3, 4));
			Console.WriteLine(MathsHelpers.RoundToDigit(d4, 4));



			Console.ReadLine();
		}
	}
}
