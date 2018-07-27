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
			double x0 = -1.34;
			double x1 = -1.34e-1;
			double x2 = -1.34e-2;
			double x3 = -1.34e-3;
			double x4 = -1.34e-4;
			double x5 = -1.34e-5;
			double x6 = -1.34e-6;
			double x7 = -1.34e-7;
			double x8 = -1.34e-8;
			double x9 = -1e-8;
			double x10 = -1e-9;
			double x11 = -1e-10;
			double x00 = -0.134;

			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x00, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x0, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x1, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x2, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x3, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x4, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x5, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x6, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x7, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x8, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x9, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x10, "V"));
			Debug.WriteLine(SIHelpers.ToSIStringExcludingSmallPrefixes(x11, "V"));
			



			Console.ReadLine();
		}
	}
}
