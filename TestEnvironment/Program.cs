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
			var prefix = SIHelpers.GetClosestPrefixExcludingSmall(1000);

			Console.WriteLine(SIHelpers.ToSIString(1000, "V"));
			Console.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(1000, "V"));
			Console.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(-1000, "V"));

			Console.ReadLine();
		}
	}
}
