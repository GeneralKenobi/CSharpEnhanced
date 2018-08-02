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
			double d = 0.00999997000009;

			Console.WriteLine(MathsHelpers.RoundToDigit(d, 4));



			Console.ReadLine();
		}
	}
}
