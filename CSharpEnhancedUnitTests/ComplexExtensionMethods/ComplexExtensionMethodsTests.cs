using CSharpEnhanced.Maths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEnhancedUnitTests
{
	[TestClass]
	public class ComplexExtensionMethodsTests
	{
		[TestMethod]
		public void RoundToTest()
		{
			// Standard rounding up, 47 + 20i to a multiple of 12
			Assert.AreEqual(new Complex(48, 24), new Complex(47, 20).RoundTo(12));

			// Standard rounding down, 40 + 11i to a multiple of 9
			Assert.AreEqual(new Complex(36, 9), new Complex(40, 11).RoundTo(9));

			// Round up & down (real & imaginary respectively), 43 + 11i to a multiple of 9
			Assert.AreEqual(new Complex(45, 9), new Complex(43, 11).RoundTo(9));

			// Round down & up (real & imaginary respectively), 38 + 14i to a multiple of 9
			Assert.AreEqual(new Complex(36, 18), new Complex(38, 14).RoundTo(9));

			// Round up for the middle value 6.75 + 2.25i to a multiple of 4.5
			Assert.AreEqual(new Complex(9, 4.5), new Complex(6.75, 2.25).RoundTo(4.5, MidpointRounding.AwayFromZero));

			// Round up for the middle value 6.75 + 2.25i to a multiple of 4.5
			Assert.AreEqual(new Complex(9, 0), new Complex(6.75, 2.25).RoundTo(4.5, MidpointRounding.ToEven));
		}
	}
}
