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
			var prefix = SIHelpers.GetClosestPrefix(0);

			Console.WriteLine(SIHelpers.ToSIString(0, "V"));
			Console.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(0, "V"));

			Console.ReadLine();
		}
	}
}
