using CSharpEnhanced.CoreClasses;
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

			double a = 180;
			double a1 = MathsHelpers.ConvertAngle(a, AngleUnit.Degrees, AngleUnit.Radians);
			double a2 = MathsHelpers.ConvertAngle(a, AngleUnit.Degrees, AngleUnit.Turns);
			double a3 = MathsHelpers.ConvertAngle(a, AngleUnit.Degrees, AngleUnit.Degrees);
			a = Math.PI;
			double a4 = MathsHelpers.ConvertAngle(a, AngleUnit.Radians, AngleUnit.Degrees);
			double a5 = MathsHelpers.ConvertAngle(a, AngleUnit.Radians, AngleUnit.Turns);
			double a6 = MathsHelpers.ConvertAngle(a, AngleUnit.Radians, AngleUnit.Radians);
			a = 0.5;
			double a7 = MathsHelpers.ConvertAngle(a, AngleUnit.Turns, AngleUnit.Degrees);
			double a8 = MathsHelpers.ConvertAngle(a, AngleUnit.Turns, AngleUnit.Degrees);
			double a9 = MathsHelpers.ConvertAngle(a, AngleUnit.Turns, AngleUnit.Turns);








			//var a = MathsHelpers.CalculateMidPoints(0, 5, 5);
			//var b = MathsHelpers.CalculateMidPoints(1, 1, 1);
			//var c = MathsHelpers.CalculateMidPoints(1, 1, 0);
			//var dd = MathsHelpers.CalculateMidPoints(1, 2, -1);




			//double val = -9.9998;

			////while(true)
			//Console.WriteLine(val.RoundToDigit(4));

			//IEnumerable<string> enn = new string[] { "Test1", "Test2" };
			//var en = enn;
			//en = en.Concat("Test3");
			//foreach(var item in enn)
			//{
			//	Console.WriteLine(item);
			//}




			//Console.WriteLine(SIHelpers.ToSIString(new Complex(Math.PI * 10000, Math.PI * 1000), "V", 4));

			//var x = new ControlledObservableCollection<string>((y) => Console.WriteLine("Added " + y), (y) => Console.WriteLine("Removed " + y));

			//x.Add("Test");
			//x[0] = "Another test";
			//x.RemoveAt(0);

			//x.Add("Test1");
			//x.Add("Test2");
			//x.Add("Test3");

			//x.Move(0, 2);

			//x.Clear();

			//var result = LinearEquations.SimplifiedGaussJordanElimination(
			//	new Complex[,] { { 1, 2, 0, 1, 0, 1 }, { 1, 1, 0, 0, 0, 1 }, { 0, 0, 0, 0, 0, 0 }, { 1, 0, 0, 0, 0, 2 }, { 0, 0, 0, 0, 0, 0 }, { 2, 1, 0, 0, 0, 0 } },
			//	new Complex[] { 0, 0, 0, 5 , 0, 4}, true); 


			//string s = "5uV";

			//var ss = SIHelpers.ParseSIString(s);

			//Complex c = new Complex(0, 0);
			//Console.WriteLine(SIHelpers.ToAltSIStringExcludingSmallPrefixes(c, "V"));
			//c = new Complex(-21000, 220);
			//Console.WriteLine(SIHelpers.ToAltSIStringExcludingSmallPrefixes(c, "Volts", true, true));
			//c = new Complex(-21000, 0);
			//Console.WriteLine(SIHelpers.ToAltSIStringExcludingSmallPrefixes(c, "Volts", true, true));
			double d = 0.010000001;
			double d1 = 10.00002000004;
			double d2 = 9.99997000009;
			double d3 = 0.00999997000009;
			double d4 = 100;
			double d5 = 0.1;
			//
			Console.WriteLine(MathsHelpers.RoundToDigit(d, 4));
			Console.WriteLine(MathsHelpers.RoundToDigit(d1, 4));
			Console.WriteLine(MathsHelpers.RoundToDigit(d2, 4));
			Console.WriteLine(MathsHelpers.RoundToDigit(d3, 4));
			Console.WriteLine(MathsHelpers.RoundToDigit(d4, 4));
			Console.WriteLine(MathsHelpers.RoundToDigit(d5, 4));



			Console.ReadLine();
		}
	}
}
