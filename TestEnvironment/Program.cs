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
			Console.WriteLine(-Math.PI);
			Console.WriteLine(MathsHelpers.RoundToDigit(-Math.PI, 5));
			Console.WriteLine();
			Console.WriteLine(-Math.PI * 10);
			Console.WriteLine(MathsHelpers.RoundToDigit(-10 * Math.PI, 5));
			Console.WriteLine();
			Console.WriteLine(-Math.PI * 100);
			Console.WriteLine(MathsHelpers.RoundToDigit(-100 * Math.PI, 5));
			Console.WriteLine();
			Console.WriteLine(-Math.PI * 1000);
			Console.WriteLine(MathsHelpers.RoundToDigit(-1000 * Math.PI, 2));
			Console.WriteLine();
			Console.WriteLine(-Math.PI / 1000);
			Console.WriteLine(MathsHelpers.RoundToDigit(-Math.PI / 1000, 5));
			Console.WriteLine();
			Console.WriteLine(-Math.PI / 1000);
			Console.WriteLine(MathsHelpers.RoundToDigit(-Math.PI / 1000, 8));
			Console.WriteLine();
			Console.WriteLine(-Math.PI / 100000);
			Console.WriteLine(MathsHelpers.RoundToDigit(-Math.PI / 1000, 8));
			Console.WriteLine();
			Console.WriteLine(-Math.PI / 1000);
			Console.WriteLine(MathsHelpers.RoundToDigit(-Math.PI / 1000, 2));



			Console.ReadLine();
		}
	}
}
