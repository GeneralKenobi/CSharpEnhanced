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
			string[] s = new string[] {
					"1141kV",
					"11.41kV",
					"11.41V",
					"11.41 V",
					"11.41kVdadaw",
					"1dwadaw1.41kV",
					"kV",
					"",
					null,					
					"11,41kV",
					"11,41V",
					"11,41 V",
					"11,41kVdadaw",
				};

			foreach(var item in s)
			{
				Console.Write(SIHelpers.TryParseSIString(item, out double result));
				Console.WriteLine("\t" + result.ToString());
			}

			Console.ReadLine();
		}
	}
}
