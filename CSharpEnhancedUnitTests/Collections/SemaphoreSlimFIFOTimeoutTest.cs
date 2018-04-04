using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpEnhanced.Synchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpEnhancedUnitTests
{
	[TestClass]
	public class SemaphoreSlimFIFOTimeoutTest
	{
		[TestMethod]
		public void CapacityTest()
		{
			SemaphoreSlimFIFOTimeout testUnit = new SemaphoreSlimFIFOTimeout(0);

			testUnit.Release(int.MaxValue);
			Assert.AreEqual(testUnit.CurrentCount, int.MaxValue);

			testUnit = new SemaphoreSlimFIFOTimeout(0, 10);

			Assert.ThrowsException<ArgumentOutOfRangeException>(() => testUnit.Release(0));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => testUnit.Release(-1));

			Assert.AreEqual(testUnit.CurrentCount, 0);
			testUnit.Release();
			Assert.AreEqual(testUnit.CurrentCount, 1);
			testUnit.Release(3);
			Assert.AreEqual(testUnit.CurrentCount, 4);
			testUnit.Release(6);
			Assert.AreEqual(testUnit.CurrentCount, 10);
			
			Assert.ThrowsException<SemaphoreFullException>(() => testUnit.Release());
			Assert.ThrowsException<SemaphoreFullException>(() => testUnit.Release(2));
		}

		[TestMethod]
		public void TestWait()
		{
			for (int i = 0; i < 100; ++i)
			{
				SemaphoreSlimFIFOTimeout testUnit = new SemaphoreSlimFIFOTimeout(1, 1);


				var firstWaiter = Task.Run(() => testUnit.Wait(1000));
				var secondWaiter = Task.Run(() => testUnit.Wait(1000));

				Assert.IsTrue(firstWaiter.Result);
				Assert.IsFalse(secondWaiter.Result);
			}
		}
	}
}
